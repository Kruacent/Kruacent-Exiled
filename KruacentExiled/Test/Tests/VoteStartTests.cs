using KruacentExiled.Misc.Features.VoteStart;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mirror.NetworkLoop;

namespace KruacentExiled.Test.Tests
{
    [TestClass]
    public class VoteStartTests
    {
        private VoteStart _votestart;
        private int _defaultMinPlayer;
        public VoteStartTests()
        {
            _votestart = new VoteStart();
            _defaultMinPlayer = Misc.MainPlugin.Configs.MinPlayerVote;
        }



        [TestCase(1)]
        [TestCase(2)]
        public void AddVote_Normal(int value)
        {
            _votestart.RemoveAllVote();
            _votestart.AddVote(value);
            if (!_votestart.DidVote(value))
            {
                Assert.Fail("should be in");
            }
        }


        [TestCase(1)]
        [TestCase(2)]
        public void RemoveVote_Normal(int value)
        {
            _votestart.RemoveAllVote();
            _votestart.AddVote(value);

            _votestart.CancelVote(value);

            if (_votestart.DidVote(value))
            {
                Assert.Fail("shouldn't be in");
            }


        }

        /// <summary>
        /// Remove vote when didn't vote
        /// </summary>
        /// <param name="value"></param>
        [TestCase(3)]
        [TestCase(4)]
        public void RemoveVoteDidntVote_ThrowsArgumentOutOfRangeException(int value)
        {
            _votestart.RemoveAllVote();

            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                _votestart.CancelVote(value);
            },"should send an exception");
        }

        /// <summary>
        /// Add a vote when already voted
        /// </summary>
        /// <param name="value"></param>
        [TestCase(3)]
        [TestCase(4)]
        public void AddVoteAlreadyVote_ThrowsException(int value)
        {
            _votestart.RemoveAllVote();

            _votestart.AddVote(value);

            Assert.Catch<ArgumentException>(() =>
            {
                _votestart.AddVote(value);
            }, "should send an exception");
        }

        // max number
        [TestCase(2)]
        [TestCase(5)]
        public void CheckVote_NormalCase_ReturnTrue(int value)
        {
            _votestart.RemoveAllVote();

            Assert.IsFalse(_votestart.CheckVote(), "should be false");

            for (int i = 0; i < 2; i++)
            {
                _votestart.AddVote(value+i);
            }

            Assert.IsTrue(_votestart.CheckVote(), "should be true");
        }
    }
}
