/*

  Copyright (c) 2002 Matt Griffith

  Permission is hereby granted, free of charge, to any person obtaining 
  a copy of this software and associated documentation files (the "Software"), 
  to deal in the Software without restriction, including without limitation 
  the rights to use, copy, modify, merge, publish, distribute, sublicense, 
  and/or sell copies of the Software, and to permit persons to whom the 
  Software is furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in 
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
  
*/
using System;
using OpenPandora.Interop;

namespace OpenPandora 
{
	
}

namespace OpenPandora.Diagnostics
{
	/// <summary>
	/// PerfTimer - based on Dean Wyant’s C++ CPerfTimer class - provides a high-resolution timer. 
	/// </summary>
	/// <remarks><p>If you try to create an instance of PerfTimer on a system that does not support the 
	/// high-resolution timer, PerfTimer will throw a HighresCounterNotSupportedException.</p>
	/// 
	/// <p>The resolution of the timer varies by system. You can determine the resolution of a given 
	/// PerfTimer instance by calling one of the ResolutionXXX methods.</p>
	/// 
	/// <p>To use PefTimer create a new instance and call Start() - or pass true to the constructor. 
	/// Call Stop() to stop a running timer. Call Start() to restart a stopped timer. 
	/// Call Start(true) to restart and reset a stopped timer. Call one of the ElapsedXXX methods 
	/// on a running or stopped timer to get the elapsed time in the units you prefer. 
	/// Check the IsRunning property to determine if the timer is running.</p>
	/// </remarks>
	/// <example>
	/// <code>
///	using System;
///	using System.Threading;
///	using MattGriffith.Diagnostics;
///
///	class TestClass
///	{
///
///		/// &lt;summary&gt;
///		/// The main entry point for the application.
///		/// &lt;/summary&gt;
///		[STAThread]
///		static void Main(string[] args)
///		{
///			int sleepCount = 750;
///			string formattedCount = sleepCount.ToString();
///
///			PerfTimer timer = new PerfTimer();
///
///			Console.WriteLine(&quot;Timer resolution: &quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.Resolution() + 
///				&quot; seconds.&quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.ResolutionMilliseconds() + 
///				&quot; milliseconds.&quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.ResolutionMicroseconds() + 
///				&quot; microseconds.&quot; + Environment.NewLine);
///
///			// Start the timer then go to sleep.
///			timer.Start();
///			Console.WriteLine(&quot;Timer started: sleeping for &quot; + formattedCount + &quot; milliseconds.&quot;);
///		
///			// Time will accumulate for this sleep because the timer is running.
///			Thread.Sleep(sleepCount);
///
///			// Pause the timer
///			timer.Stop();
///
///			Console.WriteLine(&quot;Timer paused: sleeping for &quot; + formattedCount + &quot; milliseconds.&quot;);
///		
///			// Time will not accumulate for this sleep because the timer is paused.
///			Thread.Sleep(sleepCount);
///
///			// Restart the timer and go back to sleep
///			timer.Start();
///			Console.WriteLine(&quot;Timer restarted: sleeping for &quot; + formattedCount + &quot; milliseconds.&quot;);
///
///			// Time will accumulate for this sleep because the timer is running.
///			Thread.Sleep(sleepCount);
///
///			// Stop timing and output the results.
///			timer.Stop();
///
///			Console.WriteLine(&quot;Timer stopped&quot;);
///			Console.WriteLine(Environment.NewLine + &quot;Run Time: &quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.Elapsed() + 
///				&quot; seconds.&quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.ElapsedMilliseconds() + 
///				&quot; milliseconds.&quot;);
///			Console.WriteLine(&quot;\t&quot; + timer.ElapsedMicroseconds() + 
///				&quot; microseconds.&quot;);
///
///			Console.WriteLine();
///			Console.Write(&quot;Press Enter to finish ... &quot;);
///			Console.Read();
///
///		}
///	}
///	</code>
	/// </example>
	public class PerfTimer : ICloneable
	{
	#region API Declarations
		
	#endregion

		/// <summary>
		/// Stores the frequency of the high-resolution performance counter. This value 
		/// cannot change while the system is running. Therefore it is static and made 
		/// available across all instances of PerfTimer.
		/// </summary>
		private static readonly long _frequency;
	
		/// <summary>
		/// Stores the start count or the elapsed ticks depending on context.
		/// </summary>
		protected long _start;

		/// <summary>
		/// Stores the amount of time to adjust the results of the timer to account
		/// for the time it takes to run the PerfTimer code.
		/// </summary>
		protected long _adjustment;

		/// <summary>
		/// Initializes the static members of PerfTimer.
		/// </summary>
		/// <exception cref="HighresCounterNotSupportedException">The high-resolution timer
		/// is not supported by the current system.</exception>
		static PerfTimer()
		{
			short apiReturn = Win32.QueryPerformanceFrequency(ref _frequency);
			if(0 == apiReturn)
				throw new HighresCounterNotSupportedException();
		}

		/// <summary>
		/// Initializes a new PerTimer instance without starting the timer.
		/// </summary>
		public PerfTimer() 
		{ this.Init(false); }

		/// <summary>
		/// Initializes a new PerfTimer instance and starts the timer if passed true.
		/// </summary>
		/// <param name="startTimer">Controls whether the timer is started after the
		/// PerfTimer is initialized.</param>
		public PerfTimer(bool startTimer)
		{
			this.Init(startTimer);
		}

		/// <summary>
		/// Initializes the PerfTimer members. Does all the heavy lifting for the public constructors.
		/// </summary>
		/// <param name="startTimer">Controls whether the timer is started after the
		/// PerfTimer is initialized.</param>
		protected void Init(bool startTimer)
		{
			// If the adjustment value hasn't been calculated yet then calculate it.
			if(0 == this._adjustment)
			{ 
				// Time the timer code so we will know how much of an adjustment
				// is needed.
				this._start = 0; 
				this._adjustment = 0; 
				Start(false);
				Stop(); 
				this._adjustment = this._start;
			}

			// The following needs to happen every time we initialize
			this._start = 0;
			if (startTimer)
				Start(false); 
		}

		/// <summary>
		/// Start the timer without reseting it.
		/// </summary>
		public void Start()
		{
			Start(false);
		}

		/// <summary>
		/// Start the timer.
		/// </summary>
		/// <param name="resetTimer">Controls whether the timer is reset before starting.
		/// Pass true and any new elapsed time will be added to the existing elapsed time.
		/// Pass false and any existing elapsed time is lost and only the new elapsed time
		/// is preserved.</param>
		public void Start(bool resetTimer)
		{
			long counter = 0;

			Win32.QueryPerformanceCounter(ref counter);

			if((!resetTimer) && (0 > this._start))
				this._start += counter; // We are starting with an accumulated time
			else
				this._start = counter; // We are starting from 0

		}

		/// <summary>
		/// Stop timing. Call one of the ElapsedXXX methods to get the elapsed time since Start() was
		/// called. Call Start(false) to restart the timer where you left off. Call Start(true) to
		/// restart the timer from 0.
		/// </summary>
		public void Stop() 
		{ // Stop timing. Use Start afterwards to continue
		
			if(this._start <= 0)
			{
				return;          // The timer was not running
			}

			long counter = 0;
			Win32.QueryPerformanceCounter(ref counter);
 
			this._start += -counter;          // Stopped timer keeps elapsed timer ticks as a negative 
		
			if (this._start < this._adjustment) // Do not overflow
				this._start -= this._adjustment;  // Adjust for time timer code takes to run
			else 
				this._start = 0;          // Stop must have been called directly after Start
		} 

		/// <summary>
		/// Indicates whether the timer is running or not.
		/// </summary>
		public bool IsRunning 
		{ 
			// Returns FALSE if stopped.
			get
			{
				bool result = (this._start > 0); // When < 0, holds elpased clicks
				return result;
			}
		}

		/// <summary>
		/// Creates a new PerfTimer that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Returns the number of seconds elapsed since the timer started.
		/// </summary>
		/// <returns>The number of seconds elapsed.</returns>
		public double Elapsed()
		{
			PerfTimer result = (PerfTimer)this.Clone();
			result.Stop();
			return (double)(-result._start)/(double)PerfTimer._frequency; 
		}

		/// <summary>
		/// Returns the number of milliseconds elapsed since the timer started.
		/// </summary>
		/// <returns>The number of milliseconds elapsed.</returns>
		public double ElapsedMilliseconds()
		{ 
			PerfTimer result = (PerfTimer)this.Clone();
			result.Stop();
			return (double)(-result._start * 1000)/(double)PerfTimer._frequency; 
		}

		/// <summary>
		/// Returns the number of microseconds elapsed since the timer started.
		/// </summary>
		/// <returns>The number of microseconds elapsed.</returns>
		public double ElapsedMicroseconds()
		{ 
			PerfTimer result = (PerfTimer)this.Clone();
			result.Stop();
			return (double)(-result._start * 1000000)/(double)PerfTimer._frequency; 
		}

		/// <summary>
		/// Returns the timer resolution in seconds.
		/// </summary>
		/// <returns>Then number of seconds of resolution.</returns>
		public double Resolution()   
		{ 
			return 1.0/(double)PerfTimer._frequency;
		}

		/// <summary>
		/// Returns the timer resolution in milliseconds.
		/// </summary>
		/// <returns>Then number of milliseconds of resolution.</returns>
		public double ResolutionMilliseconds() 
		{
			return 1000.0/(double)PerfTimer._frequency;
		}

		/// <summary>
		/// Returns the timer resolution in microseconds.
		/// </summary>
		/// <returns>Then number of microseconds of resolution.</returns>
		public double ResolutionMicroseconds() 
		{
			return 1000000.0/(double)PerfTimer._frequency;
		}
	}


	/// <summary>
	/// The exception that is thrown when a PerfTimer is created on a system that does not support 
	/// the high-resolution performance timer.
	/// </summary>
	public class HighresCounterNotSupportedException : ApplicationException
	{
		/// <summary>
		/// Initializes a new HighresCounterNotSupportedException with the default error
		/// message.
		/// </summary>
		public HighresCounterNotSupportedException() : base("The current hardware does not support " +
			"the high-resolution timer.")
		{}
	}

}