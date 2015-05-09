using ClipBoardLogiLCD;
using CSWPFClipboardViewer;
using dd.logilcd;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace LogiLcdClipBoardService
{
    public class ClipBoardViewer
    {
        LogiLcd LcdScreen = new LogiLcd();


        /// <summary>
        /// Next clipboard viewer window 
        /// </summary>
        private IntPtr hWndNextViewer;

        private HwndSource hWndSource;

        private bool isViewing;

        Window _window;

        public ClipBoardViewer(Window window)
        {
            _window = window;
        }

        public void OnStart()
        {
            LcdScreen.Initialize("Test", LCD_TYPE.COLOR);

            InitCBViewer();
        }

        public void OnStop()
        {
            LcdScreen.Shutdown();
            CloseCBViewer();
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);

        private const int HWND_MESSAGE = -3;
        
        private void InitCBViewer()
        {
            WindowInteropHelper wih = new WindowInteropHelper(_window);
            hWndSource = HwndSource.FromHwnd(wih.Handle);

            SetParent(wih.Handle, (IntPtr)HWND_MESSAGE); 


            hWndSource.AddHook(this.WinProc);   // start processing window messages
            hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle);   // set this window as a viewer
            isViewing = true;
        }

        private void CloseCBViewer()
        {
            // remove this window from the clipboard viewer chain
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);

            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(this.WinProc);
            //pnlContent.Children.Clear();
            isViewing = false;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        hWndNextViewer = lParam;
                    }
                    else if (hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case Win32.WM_DRAWCLIPBOARD:
                    // clipboard content changed
                    this.DrawContent();
                    // pass the message to the next viewer.
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private void DrawContent()
        {
            try
            {
                LcdScreen.SetColorTitle("");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_0, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_1, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_2, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_3, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_4, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_5, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_6, "");
                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_7, "");



                int stride = 320 * 4 + (320 % 4);

                byte[] pixelArrayPlaceHolder = new byte[stride * 240];


                LcdScreen.SetColorBackground(pixelArrayPlaceHolder);




                if (Clipboard.ContainsText())
                {

                    String text = Clipboard.GetText();

                    if (text.Length <= 17)
                    {
                        LcdScreen.SetColorTitle(text);
                    }
                    else
                    {
                        LcdScreen.SetColorTitle(text.Substring(0, 17));

                        if (text.Length <= 17 + 29)
                        {
                            LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_0, text.Substring(17));
                        }
                        else
                        {
                            LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_0, text.Substring(17, 29));

                            if (text.Length <= 17 + 29 + 29)
                            {
                                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_1, text.Substring(17 + 29));

                            }
                            else
                            {
                                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_1, text.Substring(17 + 29, 29));

                                if (text.Length <= 17 + 29 + 29 + 29)
                                {
                                    LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_2, text.Substring(17 + 29 + 29));

                                }
                                else
                                {
                                    LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_2, text.Substring(17 + 29 + 29, 29));

                                    if (text.Length <= 17 + 29 + 29 + 29 + 29)
                                    {
                                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_3, text.Substring(17 + 29 + 29 + 29));

                                    }
                                    else
                                    {
                                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_3, text.Substring(17 + 29 + 29 + 29, 29));

                                        if (text.Length <= 17 + 29 + 29 + 29 + 29 + 29)
                                        {
                                            LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_4, text.Substring(17 + 29 + 29 + 29 + 29));

                                        }
                                        else
                                        {
                                            LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_4, text.Substring(17 + 29 + 29 + 29 + 29, 29));


                                            if (text.Length <= 17 + 29 + 29 + 29 + 29 + 29 + 29)
                                            {
                                                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_5, text.Substring(17 + 29 + 29 + 29 + 29 + 29));

                                            }
                                            else
                                            {
                                                LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_5, text.Substring(17 + 29 + 29 + 29 + 29 + 29, 29));

                                                if (text.Length <= 17 + 29 + 29 + 29 + 29 + 29 + 29 + 29)
                                                {
                                                    LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_6, text.Substring(17 + 29 + 29 + 29 + 29 + 29 + 29));

                                                }
                                                else
                                                {
                                                    LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_6, text.Substring(17 + 29 + 29 + 29 + 29 + 29 + 29, 29));

                                                    if (text.Length <= 17 + 29 + 29 + 29 + 29 + 29 + 29 + 29 + 29)
                                                    {
                                                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_7, text.Substring(17 + 29 + 29 + 29 + 29 + 29 + 29 + 29));

                                                    }
                                                    else
                                                    {
                                                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_7, text.Substring(17 + 29 + 29 + 29 + 29 + 29 + 29 + 29, 29));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    // we have a file drop list in the clipboard

                    StringCollection files = Clipboard.GetFileDropList();

                    if (files.Count > 0)
                    {
                        LcdScreen.SetColorTitle(files[0].Split('\\').Last());
                    }
                    if (files.Count > 1)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_0, files[1].Split('\\').Last());
                    }
                    if (files.Count > 2)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_1, files[2].Split('\\').Last());
                    }
                    if (files.Count > 3)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_2, files[3].Split('\\').Last());
                    }
                    if (files.Count > 4)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_3, files[4].Split('\\').Last());
                    }
                    if (files.Count > 5)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_4, files[5].Split('\\').Last());
                    }
                    if (files.Count > 6)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_5, files[6].Split('\\').Last());
                    }
                    if (files.Count > 7)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_6, files[7].Split('\\').Last());
                    }
                    if (files.Count > 8)
                    {
                        LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_7, "...");
                    }

                }
                else if (Clipboard.ContainsImage())
                {

                    //pnlContent.Children.Add(new System.Windows.Controls.Image() { Source = Clipboard.GetImage(), Stretch = Stretch.Uniform });

                    BitmapSource bmp = Clipboard.GetImage();


                    LcdScreen.SetColorBackground(BitmapFromSource(bmp));
                }
                //else if (Clipboard.ContainsAudio())
                //{

                //}
                else
                {
                    LcdScreen.SetColorTitle("NOPE!");
                    LcdScreen.SetColorText(COLOR_TEXT_LINE.LINE_0, "Clipboard Content not supported");
                }

                LcdScreen.Update();
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }


        internal static class NativeMethods
        {
            // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
            public const int WM_CLIPBOARDUPDATE = 0x031D;
            public static IntPtr HWND_MESSAGE = new IntPtr(-3);

            // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
            // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        }


        internal class DeviceHelper
        {
            public static Int32 PixelsPerInch(Orientation orientation)
            {
                Int32 capIndex = (orientation == Orientation.Horizontal) ? 0x58 : 90;
                using (DCSafeHandle handle = UnsafeNativeMethods.CreateDC("DISPLAY"))
                {
                    return (handle.IsInvalid ? 0x60 : UnsafeNativeMethods.GetDeviceCaps(handle, capIndex));
                }
            }
        }

        internal sealed class DCSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private DCSafeHandle() : base(true) { }

            protected override Boolean ReleaseHandle()
            {
                return UnsafeNativeMethods.DeleteDC(base.handle);
            }
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern Boolean DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern Int32 GetDeviceCaps(DCSafeHandle hDC, Int32 nIndex);

            [DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = CharSet.Auto)]
            public static extern DCSafeHandle IntCreateDC(String lpszDriver,
                String lpszDeviceName, String lpszOutput, IntPtr devMode);

            public static DCSafeHandle CreateDC(String lpszDriver)
            {
                return UnsafeNativeMethods.IntCreateDC(lpszDriver, null, null, IntPtr.Zero);
            }
        }
    }
}
