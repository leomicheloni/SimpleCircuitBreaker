using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCircuitBreaker.Simple
{
public interface ICircuitBreaker
{
    /// <summary>
    /// Obtiene la cantidad de fallas aceptadas hasta pasar a estado Open
    /// </summary>
    int Threshold { get; }

    /// <summary>
    /// Obtine el tiempo de espera para pasar a estado Half-Open
    /// </summary>
    TimeSpan Timeout { get; }
    /// <summary>
    /// Obtiene el estado actual
    /// </summary>
    CircuitBreakerState CurrentState { get; }
    /// <summary>
    /// Permite invocar una acción utilizado el circuit breaker
    /// </summary>
    /// <param name="protectedCode">La acción que será invocada</param>
    /// <returns>El estado resultante</returns>
    CircuitBreakerState AttemptCall(Action protectedCode);
}
}
