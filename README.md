# snippets

## Win Registry API
* [RegOpenKeyEx](https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regopenkeyexa) : Opens the specified registry key. Note that key names are not case sensitive.
* [RegQueryValueEx](https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regqueryvalueexa) : Retrieves the type and data for the specified value name associated with an open registry key.
* [RegCreateKeyEx](https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regcreatekeyexa) : Creates the specified registry key. If the key already exists, the function opens it.
* [RegSetValueEx](https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regsetvalueexa) : Sets the data and type of a specified value under a registry key.
* [RegDeleteValue](https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regdeletevaluea) : Removes a named value from the specified registry key.

```
        public static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        public static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        public static int KEY_QUERY_VALUE = 0x0001;
        public static int KEY_SET_VALUE = 0x0002;
        public static int KEY_CREATE_SUB_KEY = 0x0004;
        public static int KEY_ENUMERATE_SUB_KEYS = 0x0008;
        public static int KEY_WOW64_64KEY = 0x0100;
        public static int KEY_WOW64_32KEY = 0x0200;
```
