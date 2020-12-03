using System;
using System.Runtime.InteropServices;

namespace Gopher.Cli
{
    public class Config : IConfig
    {
        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

        public string Downloads()
        {
            //"{374DE290-123F-4565-9164-39C4925E467B}", // Downloads folder special ID

            var result = SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), (uint)0, IntPtr.Zero, out IntPtr pathPtr);

            if (result >= 0)
            {
                string path = Marshal.PtrToStringUni(pathPtr);
                Marshal.FreeCoTaskMem(pathPtr);
                return path;
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public Uri Homepage()
        {
            return new Uri("gopher://sdf.org");
        }
    }
}
