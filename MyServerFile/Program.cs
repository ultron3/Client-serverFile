
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class Program
{
    static List<TcpClient> clients = new List<TcpClient>();
    static TcpListener listener;
    static int serverPort = 5550;

  public  static void Main(string[]args)
    {
        listener = new TcpListener(IPAddress.Any, serverPort);
        listener.Start();
        Console.WriteLine("Server avviato. In attesa di connessioni...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("Cliente connesso.");

            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    static void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;

        using (NetworkStream networkStream = client.GetStream())
        {
         try{

         

            
            byte[] buffer = new byte[1024];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

            string fileName = "program.exe"; // Nome temporaneo del file ricevuto
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                fileStream.Write(buffer, 0, bytesRead);
            }

            // Simuliamo la trasformazione in .inv
            string transformedFileName = Path.ChangeExtension(fileName, ".inv");
            File.Move(fileName, transformedFileName);

            Console.WriteLine("File trasformato in .inv: " + transformedFileName);
         }
         catch(IOException ex)
         {
             Console.WriteLine("Errore di lettura/scrittura: " + ex.Message);
         }
         finally{
         networkStream.Close();
            client.Close();
            clients.Remove(client);
            Console.WriteLine("Cliente disconnesso.");
         }
           
        }

        
    }

 public  static void SendFileToClients(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        
        foreach (TcpClient client in clients)
        {
            using (NetworkStream networkStream = client.GetStream())
            {
                networkStream.Write(fileData, 0, fileData.Length);
            }
        }
    }
}
