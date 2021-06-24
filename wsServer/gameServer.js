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
    
    //roomList[1]={name:"더미룸", roomNum:1, number:0};
    //roomList[2]={name:"더미룸", roomNum:2, number:0};
    refreshRoom(socket);

    socket.on("close", ()=>{
        console.log(`${socket.id}가 나감`);
        delete connectedSocket[socket.id];
        delete userList[socket.id];
        // wsService.clients.forEach(socket=>{
            
        // })
    })

    socket.on("message", msg=>{
        try{
            const data = JSON.parse(msg);

            if(data.type === "RESET_ROOM"){
                refreshRoom(socket);
            }
            if(data.type === "CREATE_ROOM"){
                if(socket.state !== SocketState.IN_LOBBY){
                    sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                    return;
                }
                let roomInfoVO = JSON.parse(data.payload);
                const roomNum = roomList.length < 1?1:Math.max(...roomList.map(x=>x.roomNum))+1;
                roomList[roomNum]={name:roomInfoVO.name, roomNum, number:1};

                socket.state = SocketState.IN_ROOM;
                socket.room = roomNum;
                socket.send(JSON.stringify({type:"CREATE_ROOM", payload:JSON.stringify({roomNum})}))
                wsService.clients.forEach(soc=>{
                    if(soc.state != SocketState.IN_LOBBY) 
                        return;
                    refreshRoom(socket);
                    //soc.send(JSON.stringify({type:"RESET_ROOM", payload:JSON.stringify(roomList)}));
                })
            }
            if(data.type === "JOIN_ROOM"){
                if(socket.state !== State.IN_LOBBY){
                    sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                    return;
                }
                let roomNum = JSON.parse(data.payload).roomNum;
                let targetRoom = roomList.find(x=>x.roomNum===roomNum);
                if(targetRoom === undefined || targetRoom.number >= 2){
                    sendError("들어갈 수 없는 방입니다.", socket);
                    return;
                }
                socket.room = roomNum;
                socket.state = SocketState.IN_ROOM;
                targetRoom.number++;
            }
        }catch(err){
            console.log(`잘못된 요청 발생 : ${msg}`);
            console.log(err);
        }
    });
});
function refreshRoom(socket)
{
    let keys = Object.keys(roomList);
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        dataList.push(roomList[keys[i]]);
    }
    socket.send(JSON.stringify({type:"RESET_ROOM", payload:JSON.stringify({dataList})}))
}
function sendError(msg, socket)
{
    socket.send(JSON.stringify({type:"POPUP_ERR", payload:msg}))
    wsService.clients.forEach(soc=>{
        if(soc.state != SocketState.IN_GAME || soc.id == socket.id) 
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