using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_ATM
{
    public class EatmArray
    {
        public int[] CardNumbers;
        public int[] PinNumbers;
        public int[] Balance;
        public int[] NumberOfTransactions;
        private int _oneTimeMaxLimit;
        private string _cardNumberPrompt;
        private string _invalidPinNumberPrompt;
        private string _inputErrorMessage; 
        private enum Action
        {
            BalanceEnquiry,
            CashWithdrawal,
            Exit
        }

        public void Start()
        { 
            var cardNumberIndex = GetCardNumberIndex(_cardNumberPrompt);
            var isValidPin = CheckIfValidPinNumber(cardNumberIndex);
            if (!isValidPin)
                Start();
            else
            {
                PerformAction(cardNumberIndex);
            }
        }

        private int GetCardNumberIndex(string prompt)
        {
            var cardNumber = TakeUserInput(prompt, _inputErrorMessage);
            var cardNumberIndex = Array.IndexOf(CardNumbers, cardNumber);
            if (cardNumberIndex < 0)
                return GetCardNumberIndex("Invalid Card Number, please enter a correct Card Number");
            return cardNumberIndex;
        }

        private bool CheckIfValidPinNumber(int index)
        {
            var pinNumber = TakeUserInput("Please enter pin number", _inputErrorMessage);
            if (PinNumbers[index].Equals(pinNumber)) 
                return true;
            Console.WriteLine(_invalidPinNumberPrompt);
            return false;
        }

        private void PerformAction(int cardNumberIndex)
        {
            var action =
                (Action)
                    TakeUserInput("Press 1 for balance enquiry \nPress 2 for cash withdrawal \nPress 3 to exit",
                        _inputErrorMessage) - 1;
            switch (action)
            {
                case Action.BalanceEnquiry:
                    BalanceEnquiry(cardNumberIndex);
                    break;
                case Action.CashWithdrawal:
                    if (NumberOfTransactions[cardNumberIndex]== 3)
                    {
                        Console.WriteLine("You have reached your daily limit of 3 transactions");
                        PerformAction(cardNumberIndex);
                    }
                    else
                        CashWithdrawal(cardNumberIndex);
                    break;
                case Action.Exit:
                    Start();
                    break;
            }
        }

        private void BalanceEnquiry(int cardNumberIndex)
        {
            Console.WriteLine("Your account balance is {0}", Balance[cardNumberIndex]);
            PerformAction(cardNumberIndex);
        }

        private void CashWithdrawal(int cardNumberIndex)
        {
            var amountWithdrawn = TakeUserInput("Please enter amount you want to withdraw", _inputErrorMessage);
            if (amountWithdrawn > _oneTimeMaxLimit)
            {
                Console.WriteLine("You have exceeded the maximum one time limit");
                PerformAction(cardNumberIndex);
            }
            else if (amountWithdrawn > Balance[cardNumberIndex])
            {
                Console.WriteLine("You dont have enough balance to make that transaction");
                PerformAction(cardNumberIndex);
            }
            else
            {
                NumberOfTransactions[cardNumberIndex]++;
                var previousBalance = Balance[cardNumberIndex];
                Balance[cardNumberIndex] = previousBalance - amountWithdrawn;
                Console.WriteLine("You have successfully withdrawn {0}. Your new account balance is {1}", amountWithdrawn, Balance[cardNumberIndex]);
                PerformAction(cardNumberIndex);
            }
        }

        private int TakeUserInput(string inputPrompt, string errorMessage)
        {
            Console.WriteLine(inputPrompt);
            var input = Console.ReadLine();
            try
            {
                return Convert.ToInt32(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(errorMessage);
                return TakeUserInput(inputPrompt, errorMessage);
            }
        }

        public void InitDefaultValues()
        {
            CardNumbers = new[] { 1, 2, 3 };
            PinNumbers = new[] { 123, 234, 456 };
            Balance = new[] { 500, 200, 800 };
            NumberOfTransactions = new[] {0, 0, 0};
            _inputErrorMessage = "You have to enter a number";
            _cardNumberPrompt = "Please enter Card Number";
            _oneTimeMaxLimit = 1000;
            _invalidPinNumberPrompt = "Invalid Pin Number";
        }
    }
}
