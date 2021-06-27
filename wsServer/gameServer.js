const WebSocket = require('ws');
const port = 36589;

const SocketState = require('./SocketState.js');
const Vector3 = require('./Vector3.js');

let socketIdx = 0;
let roomList = {};
let userList = {};
let connectedSocket = {};

const wsService = new WebSocket.Server({port}, ()=>{
    console.log(`웹 소켓이 ${port}에서 구동중`);
});

wsService.on("connection", socket=>{
    console.log('소켓 연결');

    socket.state = SocketState.IN_LOGIN;
    socket.id = socketIdx;
    connectedSocket[socketIdx] = socket;
    socket.room = -1;
    socketIdx++;

    socket.send(JSON.stringify({type:"CHAT", payload:"Hello Client"}));
    debug(socket);

    socket.on("close", ()=>{
        console.log(`소켓 연결 해제  id: ${socket.id}`);
        let roomNum = socket.room;
        console.log(`소켓 연결 해제  room: ${socket.room}`);
        if(socket.room > 0 ){
            exitRoom(socket, roomNum);
            wsService.clients.forEach(soc=>{
                if(soc.room === roomNum)
                    refreshUser(soc, roomNum);
                if(soc.state === SocketState.IN_LOBBY) 
                    refreshRoom(soc);
            })
        }
        delete connectedSocket[socket.id];
        delete userList[socket.id];
    })

    socket.on("message", msg=>{
        try{
            const data = JSON.parse(msg);
            
            if(data.type === "LOGIN"){
                let payload = JSON.parse(data.payload);
                if(payload.name === ""){
                    sendError("이름을 입력해주세요", socket);
                    return;
                }
                let userData = {socketId:socket.id, name:payload.name, roomNum:0};
                userList[socket.id] = userData;
                socket.state = SocketState.IN_LOBBY;
                socket.room = 0;
                socket.send(JSON.stringify({type:"LOGIN", payload:JSON.stringify({name:payload.name, socketId:socket.id})}))
                return;
            }
            if(data.type === "RESET_ROOM"){
                refreshRoom(socket);
            }
            if(data.type === "CREATE_ROOM"){
                if(socket.state !== SocketState.IN_LOBBY){
                    sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                    return;
                }
                let roomInfoVO = JSON.parse(data.payload);
                
                if(roomInfoVO.name === ""){
                    sendError("방이름을 입력해 주세요.", socket);
                    return;
                }
                let keys = Object.keys(roomList);
                let dataList = []; // 배열로 바꿔줌
                for(let i=0; i<keys.length; i++){
                    dataList.push(roomList[keys[i]]);
                }
                
                const roomNum = dataList.length < 1?1:Math.max(...dataList.map(x=>x.roomNum))+1;
                roomList[roomNum]={name:roomInfoVO.name, roomNum, number:1};

                socket.state = SocketState.IN_ROOM;
                socket.room = roomNum;
                if(userList[socket.id] !== undefined){
                    userList[socket.id].roomNum = roomNum;
                }

                socket.send(JSON.stringify({type:"GO_ROOM"}))

                wsService.clients.forEach(soc=>{
                    if(soc.state != SocketState.IN_LOBBY) 
                        return;
                    refreshRoom(soc);
                    //soc.send(JSON.stringify({type:"RESET_ROOM", payload:JSON.stringify(roomList)}));
                });
                refreshUser(socket, roomNum);
            }
            if(data.type === "JOIN_ROOM"){
                if(socket.state !== SocketState.IN_LOBBY){
                    sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                    return;
                }
                let roomNum = JSON.parse(data.payload).roomNum;
                let targetRoom = roomList[roomNum];

                if(targetRoom === undefined || targetRoom.number >= 2){
                    sendError("들어갈 수 없는 방입니다.", socket);
                    return;
                }
                socket.room = roomNum;
                if(userList[socket.id] !== undefined){
                    userList[socket.id].roomNum = roomNum;
                }
                socket.state = SocketState.IN_ROOM;
                targetRoom.number++;

                socket.send(JSON.stringify({type:"GO_ROOM"}))

                wsService.clients.forEach(soc=>{
                    if(soc.room !== roomNum) 
                        return;
                    refreshUser(soc, roomNum);
                })
            }
            if(data.type === "EXIT_ROOM"){
                if(socket.state !== SocketState.IN_ROOM){
                    sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
                    return;
                }
                let roomNum = JSON.parse(data.payload).roomNum;
                
                exitRoom(socket, roomNum);

                socket.send(JSON.stringify({type:"GO_LOOBY"}))

                wsService.clients.forEach(soc=>{
                    if(soc.room === roomNum)
                        refreshUser(soc, roomNum);
                    if(soc.state === SocketState.IN_LOBBY) 
                        refreshRoom(soc);
                })
            }
        }catch(err){
            console.log(`잘못된 요청 발생 : ${msg}`);
            console.log(err);
        }
    });
});
function exitRoom(socket, roomNum)
{
    let targetRoom = roomList[roomNum];

    socket.room = 0;
    if(userList[socket.id] !== undefined){
        userList[socket.id].roomNum = 0;
    }
    socket.state = SocketState.IN_LOBBY;
    targetRoom.number--;

    if(targetRoom.number === 0){
        delete roomList[roomNum];
    }
}
function refreshRoom(socket)
{
    let keys = Object.keys(roomList);
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        dataList.push(roomList[keys[i]]);
    }
    socket.send(JSON.stringify({type:"RESET_ROOM", payload:JSON.stringify({dataList})}))
}
function refreshUser(socket, roomNum)
{
    let keys = Object.keys(userList);
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        if(userList[keys[i]].roomNum === roomNum){
            dataList.push(userList[keys[i]]);
        }
    }
    //console.log(userList);
    socket.send(JSON.stringify({type:"RESET_USER", payload:JSON.stringify({dataList})}))
}
function sendError(msg, socket)
{
    socket.send(JSON.stringify({type:"ERROR", payload:msg}));
}
function roomBroadcast(msg, socket)
{
    wsService.clients.forEach(soc=>{
        if(soc.room !== roomNum || soc.id == socket.id) 
            return;
        soc.send(msg);
    })
}
function broadcast(msg, socket)
{
    wsService.clients.forEach(soc=>{
        if(soc.state != SocketState.IN_GAME || soc.id == socket.id) 
            return;
        soc.send(msg);
    })
}

function debug(socket)
{
    //roomList[1]={name:"더미룸", roomNum:1, number:0};
    //roomList[2]={name:"더미룸", roomNum:2, number:0};
    refreshRoom(socket);
}

// setInterval(()=>{
//     let keys = Object.keys(userList);
//     let dataList = []; // 전송할 배열
//     for(let i=0; i<keys.length; i++){
//         dataList.push(userList[keys[i]]);
//     }
//     wsService.clients.forEach(socket=>{
//         if(socket.state != SocketState.IN_GAME)
//             return;
//         socket.send(JSON.stringify({type:"REFRESH", payload:JSON.stringify({dataList})}))
//     })
// }, 100);