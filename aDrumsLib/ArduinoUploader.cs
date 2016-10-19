using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDrumsLib
{
    //todo: use avrdude is lighter than ide
    public class ArduinoUploader
    {
        /// <summary>
        /// path to arduino exe
        /// ex C:\Program Files (x86)\Arduino\arduino.exe
        /// </summary>
        public string UploaderExePath { get; set; }
        /// <summary>
        /// path to INO File (arduino sketch)
        /// </summary>
        public string SketchFilePath { get; set; }

        public ArduinoUploader(string s_ArduinoExePath, string s_SketchFilePath)
        {
            UploaderExePath = s_ArduinoExePath;
            SketchFilePath = s_SketchFilePath;
        }

        public void Upload()
        {
            Execute("--upload " + SketchFilePath);
        }

        private void Execute(string args)
        {
            Process P = Process.Start(UploaderExePath, args);
            P.WaitForExit();
            int result = P.ExitCode;
            switch (result)
            {
                case 0:
                default:
                    break;
                case 1:
                    throw new UploadException("Build failed or upload failed");
                case 2:
                    throw new UploadException("Sketch not found");
                case 3:
                    throw new UploadException("Invalid (argument for) commandline option");
                case 4:
                    throw new UploadException("Preference passed to --get-pref does not exist");
            }
        }

        public class UploadException : Exception { public UploadException(string Message) : base(Message) { } }
    }
}
