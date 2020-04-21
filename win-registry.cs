using System;
using System.Runtime.InteropServices;
using System.Text;

namespace registry
{
   

    class Program
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int RegOpenKeyEx( UIntPtr hKey, string subKey, int ulOptions, int samDesired,out UIntPtr hkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        public static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, out UInt32 lpType, StringBuilder lpData, ref int lpcbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(UIntPtr hKey);
        [DllImport("advapi32.dll", SetLastError = false)]
        public static extern int RegCreateKeyEx(UIntPtr hKey, string lpSubKey, int Reserved, string lpClass, RegOption dwOptions, RegSAM samDesired, ref int lpSecurityAttributes,  out UIntPtr phkResult, out int lpdwDisposition);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegSetValueEx( UIntPtr hKey, [MarshalAs(UnmanagedType.LPStr)] string lpValueName,  int Reserved,  int dwType, IntPtr lpData, int cbData);

        [DllImport("advapi32.dll", EntryPoint = "RegDeleteKeyEx", SetLastError = true)]
        public static extern int RegDeleteKeyEx(UIntPtr hKey, string lpSubKey, int samDesired, uint Reserved);

        [DllImport("advapi32.dll")]
        public static extern int RegDeleteValue(UIntPtr hKey, string lpValueName);

       

        public static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        public static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        public static int KEY_QUERY_VALUE = 0x0001;
        public static int KEY_SET_VALUE = 0x0002;
        public static int KEY_CREATE_SUB_KEY = 0x0004;
        public static int KEY_ENUMERATE_SUB_KEYS = 0x0008;
        public static int KEY_WOW64_64KEY = 0x0100;
        public static int KEY_WOW64_32KEY = 0x0200;

        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
        const uint FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
        const uint FORMAT_MESSAGE_FROM_STRING = 0x00000400;

        [Flags]
        public enum RegOption
        {
            NonVolatile = 0x0,
            Volatile = 0x1,
            CreateLink = 0x2,
            BackupRestore = 0x4,
            OpenLink = 0x8
        }

        [Flags]
        public enum RegSAM
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        public enum RegResult
        {
            CreatedNewKey = 0x00000001,
            OpenedExistingKey = 0x00000002
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registry Snippet Code - Victor Herrera - 04-2020");

            if (args.Length <= 1)
            {
                Console.WriteLine("\nModo de uso:\n\n\t-o [opcion]\n\n\t\tOpciones: \n\t\t\tck [key]\t\t\t: Crear Key\n\t\t\tcv [key] [value] [data]\t\t: Crear valor\n\t\t\tdv [key] [value]\t\t: Borra valor\n\t\t\trv [key] [value]\t\t: Leer valor");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\nIMPORTANTE: Este codigo de ejemplo solo opera en HKEY_CURRENT_USER");

            UIntPtr hKey;
            int Retorno;
            int secure = 0;

            if (args[0] == "-o")
            {
                if (args[1] == "rv")
                {

                    Retorno = RegOpenKeyEx(HKEY_CURRENT_USER, args[2], 0, KEY_QUERY_VALUE | KEY_WOW64_64KEY, out hKey);

                    int size = 1024;
                    uint type;
                    StringBuilder keyBuffer = new StringBuilder((int)size);

                    if (RegQueryValueEx(hKey, args[3], 0, out type, keyBuffer, ref size) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nValor de HKEY_CURRENT_USER\\" + args[2] + "\\" + args[3] + ":\n ");
                        Console.Write(keyBuffer.ToString());

                        RegCloseKey(hKey);
                        Console.WriteLine("\n");
                    }
                }

                if (args[1] == "ck")
                {

                    Retorno = RegCreateKeyEx(HKEY_CURRENT_USER, args[2], 0, null, RegOption.NonVolatile, RegSAM.AllAccess, ref secure, out hKey, out secure);
                    if (Retorno == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\nKey HKEY_CURRENT_USER\\" + args[2] + " creada correctamente");
                    }
                }

                if (args[1] == "cv")
                {
                    Retorno = RegCreateKeyEx(HKEY_CURRENT_USER, args[2], 0, null, RegOption.NonVolatile, RegSAM.AllAccess, ref secure, out hKey, out secure);
                    if (Retorno == 0)
                    {
                        string value = args[4];

                        int size = ((string)value).Length + 1;
                        IntPtr pData = IntPtr.Zero;
                        pData = Marshal.StringToHGlobalAnsi((string)value);

                        Retorno = RegSetValueEx(hKey, args[3], 0, 1, pData, size);

                        if (Retorno == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nValue HKEY_CURRENT_USER\\" + args[2] + "\\" + args[3] + " : " + args[4] + " creada correctamente");
                        }
                    }
                }

                if (args[1] == "dv")
                {
                    Retorno = RegOpenKeyEx(HKEY_CURRENT_USER, args[2], 0, KEY_SET_VALUE | KEY_WOW64_64KEY, out hKey);
                    if (Retorno == 0)
                    {
                        Retorno = RegDeleteValue(hKey, args[3]);
                        if (Retorno == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nValue HKEY_CURRENT_USER\\" + args[2] + "\\" + args[3] + " eliminado correctamente");
                        }

                        RegCloseKey(hKey);
                    }
                }
            }
          
            Console.ForegroundColor = ConsoleColor.Gray;
        }

      
    }
}
