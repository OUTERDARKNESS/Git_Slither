using UnityEngine;
using System.Collections;

public class Timer {
	
	public float Duration = 1f;
	private float lastTime = -1f;
	public float StartTime = 0f;
	private float one = 1f;
	private float zero = 0f;
	
	private static AnimationCurve SmoothCurve = new AnimationCurve( new Keyframe[]{ new Keyframe(0,0,0,0), new Keyframe(1,1,0,0) } );
	private static AnimationCurve SmootherCurve = new AnimationCurve( new Keyframe[]{ new Keyframe(0,0,0,0),new Keyframe(0.5f,0.5f,3,3), new Keyframe(1,1,0,0) } );	
	private static AnimationCurve CurveFastIn = new AnimationCurve( new Keyframe[]{ new Keyframe(0,0,0,2), new Keyframe(1,1,0,0) } );
	private static AnimationCurve CurveFastOut = new AnimationCurve( new Keyframe[]{ new Keyframe(0,0,0,0), new Keyframe(1,1,2,0) } );
	
	/// <summary>
	/// Gets the linear time retrun
	/// </summary>
	/// <value>
	/// The time.
	/// </value>
	public float time {
		get
		{
			
			_time = CalculateTime();
			return _time;
			
//			if ( lastTime >= one )
//			{
//				return one;	
//			}
//			else if ( lastTime <= zero )
//			{
//				return zero;
//			}
//			else
//			{
//				_time = CalculateTime();
//				return _time;
//			}
		}
		private set
		{
			time = _time;
		}
	}
	private float _time = 0f;
	
	/// <summary>
	/// Gets the time smooth time retrun
	/// </summary>
	/// <value>
	/// The time smooth.
	/// </value>
	public float timeSmooth {
		get
		{
			if(lastTime == 1f){
				return one;	
			}else{
				_timeSmooth = SmoothCurve.Evaluate( CalculateTime() );
				return _timeSmooth;
			}
		}

	}
	private float _timeSmooth = 0f;
	
	
	
	public float timeSmoother {
		get
		{
			if(lastTime == 1f){
				return one;	
			}else{
				_timeSmoother = SmootherCurve.Evaluate( CalculateTime() );
				return _timeSmoother;
			}
		}

	}
	private float _timeSmoother = 0f;
	
	/// <summary>
	/// Gets the time fast in time retrun
	/// </summary>
	/// <value>
	/// The time fast in.
	/// </value>
	public float timeFastIn {
		get
		{
			if(lastTime == 1f){
				return one;	
			}else{
				_timeFastIn = CurveFastIn.Evaluate( CalculateTime() );
				return _timeFastIn;
			}
		}

	}
	private float _timeFastIn = 0f;
	
	/// <summary>
	/// Gets the time fast out time retrun
	/// </summary>
	/// <value>
	/// The time fast out.
	/// </value>
	public float timeFastOut {
		get
		{
			if(lastTime == 1f){
				return one;	
			}else{
				_timeFastOut = CurveFastOut.Evaluate( CalculateTime() );
				return _timeFastOut;
			}
		}

	}
	private float _timeFastOut = 0f;
	
	/// <summary>
	/// Gets the actual unclamed time retrun
	/// </summary>
	/// <value>
	/// The time un clamped.
	/// </value>
	public float timeUnClamped {
		get
		{
			return CalculateTime();
		}
	}
	
	/// <summary>
	/// Gets the actual time since the Timer was instantiated.
	/// </summary>
	/// <value>
	/// The time since started.
	/// </value>
	public float timeTotal {
		get
		{
			return Time.time - StartTime;
		}
	}
	
	/// <summary>
	/// Gets the linear time inversed ( 1 - time )
	/// </summary>
	/// <value>
	/// The time inversed.
	/// </value>
	public float timeInversed
	{
		get
		{
			return (one - time);
		}
	}
	
	/// <summary>
	/// Gets the smooth time inversed ( 1 - timeSmooth )
	/// </summary>
	/// <value>
	/// The time smooth inversed.
	/// </value>
	public float timeSmoothInversed
	{
		get
		{
			return (one - timeSmooth);
		}
	}
	
	/// <summary>
	/// Gets the FastIn time inversed ( 1 - timeFastIn )
	/// </summary>
	/// <value>
	/// The time fast in inversed.
	/// </value>
	public float timeFastInInversed
	{
		get
		{
			return (one - timeFastIn);
		}
	}
	
	/// <summary>
	/// Gets the FastOut time inversed ( 1 - timeFastOut )
	/// </summary>
	/// <value>
	/// The time fast out inversed.
	/// </value>
	public float timeFastOutInversed
	{
		get
		{
			return (one - timeFastOut);
		}
	}
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Timer"/> class with default Duration: 1 sec
	/// </summary>
	public Timer(){
		StartTime = Time.time;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Timer"/> class.
	/// </summary>
	/// <param name='duration'>
	/// Duration.
	/// </param>	
	public Timer (float duration) {
		_time = zero;
		Duration = duration;
		StartTime = Time.time;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Timer"/> class.
	/// Time will return 0 untill delay delay has been reached
	/// </summary>
	/// <param name='duration'>
	/// Duration.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public Timer (float duration, float delay) {
		_time = zero;
		Duration = duration;
		StartTime = Time.time + delay;
	}
	
	private float CalculateTime (){
		if( Duration == zero ){ return one; }
		float ThisVal = 0f;
		ThisVal = (Time.time - StartTime) / Duration;
		lastTime = ThisVal;
		return ThisVal;
	}

}
