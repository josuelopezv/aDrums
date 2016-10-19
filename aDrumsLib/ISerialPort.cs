using System.Text;

namespace aDrumsLib
{
    public interface ISerialPort
    {
        ////Stream BaseStream { get; }
        int BaudRate { get; set; }
        //bool BreakState { get; set; }
        int BytesToRead { get; }
        //int BytesToWrite { get; }
        //bool CDHolding { get; }
        //bool CtsHolding { get; }
        //int DataBits { get; set; }
        //bool DiscardNull { get; set; }
        //bool DsrHolding { get; }
        bool DtrEnable { get; set; }
        //Encoding Encoding { get; set; }
        ////Handshake Handshake { get; set; }
        bool IsOpen { get; }
        //string NewLine { get; set; }
        ////Parity Parity { get; set; }
        //byte ParityReplace { get; set; }
        string PortName { get; set; }
        //int ReadBufferSize { get; set; }
        //int ReadTimeout { get; set; }
        //int ReceivedBytesThreshold { get; set; }
        //bool RtsEnable { get; set; }
        ////StopBits StopBits { get; set; }
        //int WriteBufferSize { get; set; }
        //int WriteTimeout { get; set; }
        
        void Close();
        //void DiscardInBuffer();
        //void DiscardOutBuffer();
        void Open();
        ////string[] GetPortNames();
        //int Read(char[] buffer, int offset, int count);
        int Read(byte[] buffer, int offset, int count);
        int ReadByte();
        //int ReadChar();
        string ReadExisting();
        //string ReadLine();
        //string ReadTo(string value);
        //void Write(string str);
        //void Write(char[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
        //void WriteLine(string str);
    }
}