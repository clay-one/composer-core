using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CompositionByConstructor
{
    [TestClass]
    public class ConstructorResolutionConfigurationTest
    {
        [TestMethod]
        public void ExplicitPolicyDefaultConstructorWithoutAttribute()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void ExplicitPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void AttributeOverridesPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DefaultConstructorPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithDefault()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithoutDefaultSingleConstructor()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithoutDefaultMultipleConstructors()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostParametersPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostParametersPolicyMultipleMatch()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void LeastParametersPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void LeastParametersPolicyMultipleMatch()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostResolvablePolicyRequiredParams()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostResolvablePolicyOptionalParams()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostResolvablePolicyNoMatch()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void MostResolvablePolicyMultipleMatch()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void CustomPolicy()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void SetPolicyWithAttribute()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void ConstructorResolutionPolicyIsInherited()
        {
            Assert.Fail();
        }
    }
}