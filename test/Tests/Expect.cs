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

                Assert.Fail("Exception of type {0} expected; got none exception", typeof(TException).Name);
            }
            catch (TException)
            {
                // Expectation is fulfilled.
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception of type {0} expected; got exception of type {1}", typeof(TException).Name, ex.GetType().Name);               
            }
        }
    }
}