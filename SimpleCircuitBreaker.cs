using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCircuitBreaker.Simple
{
public enum CircuitBreakerState
{
    Close,
    Open,
    HalfOpen
}

public class SimpleCircuitBreaker : ICircuitBreaker
{
    private int threshold;
    private TimeSpan timeout;
    private int attemptCounter;
    private DateTime failureTime;
    private CircuitBreakerState currentState = CircuitBreakerState.Close;

    public int Threshold => this.threshold;
    public TimeSpan Timeout => this.timeout;
    public CircuitBreakerState CurrentState => this.currentState;

    public SimpleCircuitBreaker(int threshold, TimeSpan timeout)
    {
        this.threshold = threshold;
        this.timeout = timeout;
    }

    public CircuitBreakerState AttemptCall(Action protectedCode)
    {
        switch (this.currentState)
        {
            case CircuitBreakerState.Close:

                try
                {
                    protectedCode();
                }
                catch (Exception)
                {
                    this.attemptCounter++;
                    if (this.attemptCounter > this.threshold)
                    {
                        this.failureTime = DateTime.Now;
                        this.attemptCounter = 0;
                        this.currentState = CircuitBreakerState.Open;
                    }
                }

                break;
            case CircuitBreakerState.Open:
                if (this.failureTime.Add(this.timeout) > DateTime.Now)
                {
                    this.currentState = CircuitBreakerState.HalfOpen;
                }
                break;
            case CircuitBreakerState.HalfOpen:
                try
                {
                    protectedCode();
                }
                catch (Exception)
                {
                    this.currentState = CircuitBreakerState.Open;
                }
                break;
        }

        return this.currentState;
    }
}
}
