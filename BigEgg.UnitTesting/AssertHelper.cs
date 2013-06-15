using System;

namespace BigEgg.UnitTesting
{
    /// <summary>
    /// This class contains assert methods which can be used in unit tests.
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Asserts that the execution of the provided action throws the specified exception.
        /// </summary>
        /// <typeparam name="T">The expected exception type</typeparam>
        /// <param name="action">The action to execute.</param>
        public static void ExpectedException<T>(Action action) where T : Exception
        {
            if (action == null) { throw new ArgumentNullException("action"); }

            bool expectedExceptionThrown = false;
            Exception thrownException = null;
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(T))
                {
                    expectedExceptionThrown = true;
                }
                thrownException = e;
            }

            if (!expectedExceptionThrown)
            {
                if (thrownException != null)
                {
                    throw new AssertException(string.Format(null,
                        "Test method threw exception {0}, but exception {1} was expected. "
                        + "Exception message: {0}: {2}",
                        thrownException.GetType().Name, typeof(T).Name, thrownException.Message));
                }
                else
                {
                    throw new AssertException(string.Format(null,
                        "Test method did not throw expected exception {0}", typeof(T).Name));
                }
            }
        }
    }
}
