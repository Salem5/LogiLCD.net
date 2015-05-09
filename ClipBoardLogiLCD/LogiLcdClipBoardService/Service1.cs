using dd.logilcd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogiLcdClipBoardService
{
    public partial class Service1 : ServiceBase
    {
        LogiLcd LcdScreen = new LogiLcd();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LcdScreen.Initialize("Test", LCD_TYPE.COLOR);

            InitCBViewer();
        }

        protected override void OnStop()
        {
            LcdScreen.Shutdown();
            CloseCBViewer();
        }

        private void InitCBViewer()
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            hWndSource = HwndSource.FromHwnd(wih.Handle);

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
            pnlContent.Children.Clear();
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
                pnlContent.Children.Clear();


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
                    // we have some text in the clipboard.
                    TextBox tb = new TextBox();
                    tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    tb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    tb.Text = Clipboard.GetText();
                    tb.IsReadOnly = true;
                    tb.TextWrapping = TextWrapping.NoWrap;
                    pnlContent.Children.Add(tb);

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
                    ListBox lb = new ListBox();
                    StringCollection files = Clipboard.GetFileDropList();
                    pnlContent.Children.Add(lb);

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
                else if (Clipboard.ContainsAudio())
                {

                }
                else
                {
                    Label lb = new Label();
                    lb.Content = "The type of the data in the clipboard is not supported by this sample.";
                    pnlContent.Children.Add(lb);
                }
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




        public class VisualUtility
        {
            public static BitmapSource CreateBitmapFromVisual(Double width,
                    Double height,
                    Visual visualToRender,
                    Boolean undoTransformation)
            {
                if (visualToRender == null)
                {
                    return null;
                }

                // The PixelsPerInch() helper method is used to read the screen DPI setting.
                // If you need to create a bitmap with a specified resolution, you could directly
                // pass the specified dpiX and dpiY values to RenderTargetBitmap constructor.
                RenderTargetBitmap bmp = new RenderTargetBitmap((Int32)Math.Ceiling(width),
                                                                (Int32)Math.Ceiling(height),
                                                                (Double)DeviceHelper.PixelsPerInch(Orientation.Horizontal),
                                                                (Double)DeviceHelper.PixelsPerInch(Orientation.Vertical),
                                                                PixelFormats.Pbgra32);

                // If we want to undo the transform, we could use VisualBrush trick.
                if (undoTransformation)
                {
                    DrawingVisual dv = new DrawingVisual();
                    using (DrawingContext dc = dv.RenderOpen())
                    {
                        VisualBrush vb = new VisualBrush(visualToRender);
                        dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
                    }
                    bmp.Render(dv);
                }
                else
                {
                    bmp.Render(visualToRender);
                }

                return bmp;
            }
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
