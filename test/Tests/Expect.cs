using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests
{
    public static class Expect
    {
        public static void ToThrow<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                // Expectation is fulfilled.
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception of type {typeof(TException).Name} expected; " +
                            $"got exception of type {ex.GetType().Name}.");               
            }

            Assert.Fail($"Exception of type {typeof(TException).Name} expected; but no exceptions are thrown.");
        }
    }
}