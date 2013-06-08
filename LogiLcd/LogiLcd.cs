﻿#region License
/**********************************************************************
 * LogiLCD.net - Copyright (C) 2013 by Daniel Drywa (daniel@drywa.me)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 ***********************************************************************
 * Logitech Gaming Lcd SDK
 * Copyright (C) 2013 Logitech Inc. All Rights Reserved
 * 
 * Logitech Gaming LCD SDK is either a registered trademark or trademark of Logitech in the United States and/or other countries. 
 * All other trademarks are the property of their respective owners.
 * 
 **********************************************************************/
#endregion

#region Using Directives
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
#endregion

namespace dd.logilcd {
	[Flags]
	public enum LCD_TYPE : int {
		MONO	= 0x00000001,
		COLOR	= 0x00000002
	}

	[Flags]
	public enum MONO_BUTTON : int {
		BUTTON_0 = 0x00000001,
		BUTTON_1 = 0x00000002,
		BUTTON_2 = 0x00000004,
		BUTTON_3 = 0x00000008
	}

	[Flags]
	public enum COLOR_BUTTON : int {
		BUTTON_LEFT		= 0x00000100,
		BUTTON_RIGHT	= 0x00000200,
		BUTTON_OK		= 0x00000400,
		BUTTON_CANCEL	= 0x00000800,
		BUTTON_UP		= 0x00001000,
		BUTTON_DOWN		= 0x00002000,
		BUTTON_MENU		= 0x00004000
	}

	public enum MONO_TEXT_LINE : int {
		LINE_0 = 0,
		LINE_1 = 1,
		LINE_2 = 2,
		LINE_3 = 3
	}

	public enum COLOR_TEXT_LINE : int {
		LINE_0 = 0,
		LINE_1 = 1,
		LINE_2 = 2,
		LINE_3 = 3,
		LINE_4 = 4,
		LINE_5 = 5,
		LINE_6 = 6,
		LINE_7 = 7
	}

	public class LogiLcd : IDisposable {
		public const int	MonoWidth	= 160;
		public const int	MonoHeight	= 43;
		public const int	ColorWidth	= 320;
		public const int	ColorHeight	= 240;

		private bool		isInitialized	= false;
		private bool		isDisposed		= false;
		private string		name			= string.Empty;

		/// <summary>
		/// Creates a new instance of the LogiLCD interface class
		/// </summary>
		public LogiLcd() {
		}

		/// <summary>
		/// The LogiLcd finalizer
		/// </summary>
		~LogiLcd() {
			Dispose( false );
		}

		/// <summary>
		/// Returns the name that has been set for this applet.
		/// </summary>
		public string AppletName {
			get {
				return name;
			}
		}

		/// <summary>
		/// Returns true if the LCD Screen has been initialized.
		/// </summary>
		public bool IsInitialized {
			get {
				return isInitialized;
			}
		}

		/// <summary>
		/// Returns true if the object is disposed.
		/// </summary>
		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}

		/// <summary>
		/// Initializes the LogiLcd system. You must call this function prior to any other function.
		/// </summary>
		/// <param name="friendlyName">the name of your applet, you can’t change it after initialization.</param>
		/// <param name="lcdType">defines the type of your applet lcd target. Is has to e of type LCD_TYPE.</param>
		/// <returns>true if the initialization was successful</returns>
		public bool Initialize( string friendlyName, LCD_TYPE lcdType ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( isInitialized ) {
				Shutdown();
			}

			name = friendlyName;
			int		type	= ( int )lcdType;
			bool	result	= NativeMethods.LogiLcdInit( name, type );

			isInitialized = result;
			return result;
		}
		
		/// <summary>
		/// Checks if a specific LCD_TYPE is connected to the system.
		/// </summary>
		/// <param name="lcdType">The LCD_TYPE for which we want to check the connection.</param>
		/// <returns>true if the LCD_TYPE is connected to the system</returns>
		public bool IsConnected( LCD_TYPE lcdType ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			int type = ( int )lcdType;
			return NativeMethods.LogiLcdIsConnected( type );
		}

		/// <summary>
		/// Checks if a specific button of the Mono LCD is pressed.
		/// </summary>
		/// <param name="button">The button whe want to check. It has to be of type MONO_BUTTON.</param>
		/// <returns>true if the specific button is pressed.</returns>
		public bool IsMonoButtonPressed( MONO_BUTTON button ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			int btn = ( int )button;
			return NativeMethods.LogiLcdIsButtonPressed( btn );
		}

		/// <summary>
		/// Checks if a specific button of the Color LCD is pressed.
		/// </summary>
		/// <param name="button">The button whe want to check. It has to be of type COLOR_BUTTON.</param>
		/// <returns>true if the specific button is pressed.</returns>
		public bool IsColorButtonPressed( COLOR_BUTTON button ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			int btn = ( int )button;
			return NativeMethods.LogiLcdIsButtonPressed( btn );
		}

		/// <summary>
		/// Displays a speific line of text on the Mono LCD.
		/// </summary>
		/// <param name="line">The line index where we want the text to show up. It has to be of Type MONO_TEXT_LINE</param>
		/// <param name="text">The line of text we want to show on the screen</param>
		/// <returns>true if the text line has been set</returns>
		public bool SetMonoText( MONO_TEXT_LINE line, string text ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			int i = ( int )line;
			return NativeMethods.LogiLcdMonoSetText( i, text );
		}

		/// <summary>
		/// Displays a title text on the Color LCD.
		/// </summary>
		/// <param name="text">The title text we want to show.</param>
		/// <param name="r">The Red color bit. Should be between 0 and 255.</param>
		/// <param name="g">The Green color bit. Should be between 0 and 255.</param>
		/// <param name="b">The Blue color bit. Should be between 0 and 255.</param>
		/// <returns></returns>
		public bool SetColorTitle( string text, int r = 255, int g = 255, int b = 255 ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			r = Math.Max( 0, Math.Min( r, 255 ) );
			g = Math.Max( 0, Math.Min( g, 255 ) );
			b = Math.Max( 0, Math.Min( b, 255 ) );
			return NativeMethods.LogiLcdColorSetTitle( text, r, g, b );
		}

		/// <summary>
		/// Displays a speific line of text on the Color LCD.
		/// </summary>
		/// <param name="line">The line index where we want the text to show up. It has to be of Type COLOR_TEXT_LINE</param>
		/// <param name="text">The line of text we want to show on the screen</param>
		/// <param name="r">The Red color bit. Should be between 0 and 255.</param>
		/// <param name="g">The Green color bit. Should be between 0 and 255.</param>
		/// <param name="b">The Blue color bit. Should be between 0 and 255.</param>
		/// <returns>true if the text line has been set</returns>
		public bool SetColorText( COLOR_TEXT_LINE line, string text, int r = 255, int g = 255, int b = 255 ) {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			r = Math.Max( 0, Math.Min( r, 255 ) );
			g = Math.Max( 0, Math.Min( g, 255 ) );
			b = Math.Max( 0, Math.Min( b, 255 ) );
			int i = ( int )line;
			return NativeMethods.LogiLcdColorSetText( i, text, r, g, b );
		}

		/// <summary>
		/// Updates the whole LCD Screen.
		/// You have to call this function every frame of your main loop, to keep the lcd updated.
		/// </summary>
		public void Update() {
			if ( isDisposed ) {
				throw new ObjectDisposedException( "LogiLcd" );
			}

			if ( !isInitialized ) {
				throw new InvalidOperationException( "You have to call LogiLcd.Initialize() first." );
			}

			NativeMethods.LogiLcdUpdate();
		}

		/// <summary>
		/// Shuts down the LogiLCD System and frees all the resources.
		/// </summary>
		public void Shutdown() {
			NativeMethods.LogiLcdShutdown();
			this.isInitialized = false;
		}

		/// <summary>
		/// Disposes the objects and also calls Shutdown().
		/// </summary>
		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Disposes the objects and also calls Shutdown().
		/// </summary>
		/// <param name="disposing">true if this method has been called by hand. false if not.</param>
		protected virtual void Dispose( bool disposing ) {
			if ( !isDisposed ) {
				if ( disposing ) {
					// Clean managed resources
				}

				// Clean unmanaged resources
				Shutdown();
				isDisposed = true;
			}
		}
	}
}
