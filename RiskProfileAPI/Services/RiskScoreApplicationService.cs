using RiskProfileAPI.Enums;
using RiskProfileAPI.Models;
using RiskProfileAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiskProfileAPI.Services
{
    public class RiskScoreApplicationService
    {       
        public ScoreResultViewModel getUserScore(User user)
        {
            user.BaseScore = calculateBaseScore(user.Risk_Questions);

            //If the user is under 30 years old, deduct 2 risk points from all lines of insurance.
            if (user.Age < 30) user.BaseScore -= 2;

            //If she is between 30 and 40 years old, deduct 1.
            if (user.Age > 30 && user.Age < 40) user.BaseScore -= 1;

            //If her income is above $200k, deduct 1 risk point from all lines of insurance.
            if(user.Income > 200000) user.BaseScore -= 1;

            return new ScoreResultViewModel()
            {
                Auto = AutoStatus(user),
                Disability = DisabilityStatus(user),
                Home = HomeStatus(user),
                Life = LifeStatus(user)
            };
        }

        private string DisabilityStatus(User user)
        {
            var disabilityScore = user.BaseScore;
            //If the user doesn’t have income, vehicles or houses, she is ineligible for disability
            //If the user is over 60 years old, she is ineligible for disability
            if (user.Income < 1 || user.Vehicle.Year == 0 || user.House.Ownership_Status == HouseStatus.none || user.Age > 60)
            {
                return "ineligible";
            }
            else
            {
                //If the user's house is mortgaged, add 1 risk point to her home score and add 1 risk point to her disability score.
                if(user.House.Ownership_Status == HouseStatus.mortgaged) disabilityScore += 1;

                //If the user has dependents, add 1 risk point to both the disability and life scores.
                if (user.Dependents > 0) disabilityScore += 1;

                //If the user is married, add 1 risk point to the life score and remove 1 risk point from disability.
                if(user.Marital_Status == MaritalStatus.married) disabilityScore -= 1;

                return getScoreText(disabilityScore);
            }
                
        }

        private string AutoStatus(User user)
        {
            var autoScore = user.BaseScore;
            //If the user doesn’t have income, vehicles or houses, she is ineligible for auto
            if(user.Income < 1 || user.Vehicle.Year == 0 || user.House.Ownership_Status == HouseStatus.none)
            {
                return "ineligible";
            }
            //If the user's vehicle was produced in the last 5 years, add 1 risk point to that vehicle’s score.
            if(user.Vehicle.Year >= DateTime.Now.Year - 5) autoScore += 1;

            return getScoreText(autoScore);
        }

        private string HomeStatus(User user)
        {
            var homeScore = user.BaseScore;
            //If the user doesn’t have income, vehicles or houses, she is ineligible for home
            if (user.Income < 1 || user.Vehicle.Year == 0 || user.House.Ownership_Status == HouseStatus.none)
            {
                return "ineligible";
            }
            //If the user's house is mortgaged, add 1 risk point to her home score and add 1 risk point to her disability score.
            if(user.House.Ownership_Status == HouseStatus.mortgaged) homeScore += 1;

            return getScoreText(homeScore);
        }

        private string LifeStatus(User user)
        {
            var lifeScore = user.BaseScore;

            //If the user is over 60 years old, she is ineligible for life insurance.
            if (user.Age > 60)
            {
                return "ineligible";
            }

            //If the user has dependents, add 1 risk point to both the disability and life scores.
            if (user.Dependents > 0) lifeScore += 1;

            //If the user is married, add 1 risk point to the life score and remove 1 risk point from disability.
            if (user.Marital_Status == MaritalStatus.married) lifeScore += 1;

            return getScoreText(lifeScore);
        }

        private int calculateBaseScore(List<bool> answers)
        {
            var baseScore = 0;

            foreach(bool answer in answers)
            {
                if (answer)
                {
                    baseScore++;
                }
            }

            return baseScore;
        }

        private string getScoreText(int score)
        {
            if (score <= 0) return "economic";
            if (score > 0 && score < 3) return "regular";
            if (score > 2) return "responsible";

            return null;
        }
    }
}





