using Dcf.Wwp.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.DataAccess.Model
{
    [TestClass]
    public class ParticipantTest
    {
        private readonly Participant _participant  = new Participant();
        private const    string      FirstName     = "BOB";
        private const    string      MiddleInitial = "B";
        private const    string      LastName      = "ROSS";
        private const    string      SuffixName    = "JR";

        [TestInitialize]
        public void Initialize()
        {
            _participant.FirstName     = string.Empty;
            _participant.MiddleInitial = string.Empty;
            _participant.LastName      = string.Empty;
            _participant.SuffixName    = string.Empty;
        }

        [TestMethod]
        public void DisplayNameStartsWithFirstNameInUpperCaseFormat()
        {
            _participant.FirstName = FirstName.ToLower();
            Assert.IsTrue(_participant.DisplayName.StartsWith(FirstName));
        }

        [TestMethod]
        public void DisplayNameStartsWithFirstNameAndRemovesWhiteSpaces()
        {
            _participant.FirstName = $"   {FirstName}   ";
            Assert.IsTrue(_participant.DisplayName.StartsWith(FirstName));
        }

        [TestMethod]
        public void DisplayNameHasMiddleInitialOneSpaceAfterFirstName()
        {
            _participant.FirstName     = FirstName;
            _participant.MiddleInitial = MiddleInitial;
            Assert.IsTrue(_participant.DisplayName.StartsWith($"{FirstName} {MiddleInitial}."));
        }

        [TestMethod]
        public void DisplayNameHasCapitalizedMiddleInitialFollowedByADot()
        {
            _participant.FirstName     = FirstName;
            _participant.MiddleInitial = MiddleInitial.ToLower();
            Assert.IsTrue(_participant.DisplayName.StartsWith($"{FirstName.ToUpper()} {MiddleInitial}."));
        }

        [TestMethod]
        public void DisplayNameHasNullMiddleInitial()
        {
            _participant.FirstName     = FirstName;
            _participant.MiddleInitial = null;
            Assert.AreEqual(FirstName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameMiddleInitialIsASpaceReturnsOnlyFirstName()
        {
            _participant.FirstName     = FirstName;
            _participant.MiddleInitial = " ";
            Assert.AreEqual(FirstName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameLastNameHasWhiteSpaceReturnsWithoutWhiteSpace()
        {
            _participant.LastName = $"   {LastName}   ";
            Assert.AreEqual(LastName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameLastNameHasLowerCaseReturnsUpperCase()
        {
            _participant.LastName = LastName.ToLower();
            Assert.AreEqual(LastName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameCompleteHappyPathWorks()
        {
            _participant.FirstName     = FirstName;
            _participant.MiddleInitial = MiddleInitial;
            _participant.LastName      = LastName;
            _participant.SuffixName    = SuffixName;
            Assert.AreEqual($"{FirstName} {MiddleInitial}. {LastName} {SuffixName}", _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameHasNullSuffixIgnoresSuffix()
        {
            _participant.SuffixName = null;
            Assert.AreEqual(string.Empty, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameHasEmptySuffixIgnoresSuffix()
        {
            Assert.AreEqual(string.Empty, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameHasWhiteSpaceInSuffixTrimsWhiteSpaces()
        {
            _participant.SuffixName = $" {SuffixName}  ";
            Assert.AreEqual(SuffixName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameHasLowerCaseSuffixReturnsUpperCasedSuffix()
        {
            _participant.SuffixName = SuffixName.ToLower();
            Assert.AreEqual(SuffixName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameFirstNameHasMixedCasesReturnsUpperCasedFirstName()
        {
            _participant.FirstName = "bOb";
            Assert.AreEqual(FirstName, _participant.DisplayName);
        }

        [TestMethod]
        public void DisplayNameLastNameHasMixedCasesReturnsUpperCasedLastName()
        {
            _participant.LastName = "rOsS";
            Assert.AreEqual(LastName, _participant.DisplayName);
        }
    }
}
