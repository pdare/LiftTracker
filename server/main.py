import threading
import socket
import json

server_run = True
server_listening = False

def end_server():
    global server_run
    while server_run:
        message = input("enter 'close' to stop server")
        if message == "close":
            host = "127.0.0.1"
            port = 5000

            client_socket = socket.socket()
            client_socket.connect((host, port))
            client_socket.close()
            server_run = False


def server_program():
    #global server_listening
    print("waiting for connection")
    host = "127.0.0.1"
    port = 5000

    server_socket = socket.socket()
    server_socket.bind((host, port))

    server_socket.listen(2)
    #server_listening = True
    conn, address = server_socket.accept()
    print("connection from: " + str(address))

    while True:
        data = conn.recv(1024).decode()
        if not data:
            break
        #print("from connected user: " + str(data))
        if str(data) == "get lifts":
            data = send_lifts()
        elif str(data) == "sent lifts":
            print('workout recieved')
        else:
            print(data)
        conn.send(data.encode())
    conn.close()

def send_lifts():
    return "deadlift 265 3x5"

def receive_lifts():
    pass

if __name__ == "__main__":
    #global server_listening
    t1 = threading.Thread(target=end_server, args=())
    t1.start()

    while server_run:
        t2 = threading.Thread(target=server_program, args=())
        t2.start()
        t2.join()

    t1.join()

    print("done")