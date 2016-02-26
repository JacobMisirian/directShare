# directShare
Tiny P2P File Sharing in C#

## How it Works
DirectShare works as a client server linking through a network
stream to send data. The server UI exists as a command line where
files can be sent to different clients which connect to it. When
a file is sent to the client by the server it first writes the
amount of bytes as a string to the stream. The client recieves this
number and throws an event asking the user if they want to accept
the file, if so it reads that amount of bytes from the stream and
saves them to disk. The pros of this system are that everything
from sending to recieving to saving operates on a stream, meaning
that if you are sending large files at no time will the entire file
be hogging up RAM.

## The Protocol
DirectShare protocol is very simple. When the server sends the length
of the file to the client it writes this to the stream:
```
[BYTE_SIZE] + "\r\n"
```

From there it writes each byte of the file to the stream.

## Sample Run:
Server:
```
$ ./DirectShare.exe -s 127.0.0.1 1337
> Client connected from 127.0.0.1. Check client list with list command

> send 0 /home/reagan/Hassium/src/Hassium/bin/Debug/Hassium.exe
>
```

Client:
```
$ ./DirectShare.exe -c 127.0.0.1 1337
Data of size 305664 recieved! Accept? y/n 
y
Enter path to save location: 
/home/reagan/has.exe
```
