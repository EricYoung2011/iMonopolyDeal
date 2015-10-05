﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MonopolyDealServer
{
    /// <summary>
    /// Interaction logic for playerTurn.xaml
    /// </summary>
    public partial class playerTurn : Window
    {
        public playerTurn(int i)
        {
            InitializeComponent();
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            //Table_Money.IsEnabled = false;
            //Table_Properties.IsEnabled = false;
            buttonPlayer.Content = "Player " + (i + 1);
            //this.Show();
        }

        public void beginTurn(int player)
        {
            MainWindow.playNum = 0;
            MainWindow.stage = turnStage.begin;
            setupEvents(player);
            updateUniversalPrompt(MainWindow.playerNames[player] + " began his turn");
            if (MainWindow.AllHands[MainWindow.playerNum].Count > 0)
            {
                drawCards(2);
            }
            else
            {
                updateUniversalPrompt(player.ToString() + " got to draw 5 cards this turn.");
                drawCards(5);
            }
            showTable();
            decidePlayType();
        }

        //NEED TO ADD TO CLIENT
        public void setupEvents(int playerToEnable)
        {
            //for (int i = 0; i < MainWindow.numOfPlayers; i++)
            //{
            //    if (i == playerToEnable)
            //    {
            //        MainWindow.playerTurns[i].Hand.MouseDoubleClick += MainWindow.Hand_MouseDoubleClick;
            //        MainWindow.playerTurns[i].Table_Properties.SelectionChanged += MainWindow.Table_Properties_SelectionChanged;
            //        MainWindow.playerTurns[i].Table_Money.SelectionChanged += MainWindow.Table_Money_SelectionChanged;
            //        MainWindow.playerTurns[i].Table_Properties.MouseDoubleClick += MainWindow.Table_Properties_MouseDoubleClick;
            //        for (int j = 0; j < MainWindow.numOfPlayers; j++)
            //        {
            //            if (j != i)
            //            {
            //                MainWindow.otherTable_Properties[i][j].MouseDoubleClick += MainWindow.OtherPlayer_Properties_MouseDoubleClick;
            //                MainWindow.otherNames[i][j].Click += MainWindow.OtherPlayer_Click;
            //            }
            //        }
            //    }
            //    else //everyone else
            //    {
            //        MainWindow.playerTurns[i].Hand.MouseDoubleClick -= MainWindow.Hand_MouseDoubleClick;
            //        MainWindow.playerTurns[i].Table_Properties.SelectionChanged -= MainWindow.Table_Properties_SelectionChanged;
            //        MainWindow.playerTurns[i].Table_Money.SelectionChanged -= MainWindow.Table_Money_SelectionChanged;
            //        MainWindow.playerTurns[i].Table_Properties.MouseDoubleClick -= MainWindow.Table_Properties_MouseDoubleClick;
            //        for (int j = 0; j < MainWindow.numOfPlayers; j++)
            //        {
            //            if (j != i)
            //            {
            //                MainWindow.otherTable_Properties[i][j].MouseDoubleClick -= MainWindow.OtherPlayer_Properties_MouseDoubleClick;
            //                MainWindow.otherNames[i][j].Click -= MainWindow.OtherPlayer_Click;
            //            }
            //        }
            //    }
            //}
        }

        public void drawCards(int numToDraw)
        {
            if (MainWindow.deckShuffled.Count < numToDraw)
            {
                MainWindow.shuffleDiscards();
            }
            if (MainWindow.deckShuffled.Count < numToDraw) //Not enough cards to draw... Lol, just in case...
            {
                drawCards(MainWindow.deckShuffled.Count);
            }
            else
            {
                for (int i = 0; i < numToDraw; i++)
                {
                    MainWindow.AllHands[MainWindow.playerNum].Add(MainWindow.deckShuffled[0]);
                    MainWindow.deckShuffled.RemoveAt(0);
                }
            }
            for (int player = 0; player < MainWindow.numOfPlayers; player++)
            {
                MainWindow.playerTurns[player].deckCountDisplay.Content = MainWindow.deckShuffled.Count;
            }
        }

        public void showTable()
        {
            MainWindow.sortMoney();
            for (int player = 0; player < MainWindow.numOfPlayers; player++)
            {
                MainWindow.playerTurns[player].Hand.Items.Clear();
                foreach (Card card in MainWindow.AllHands[player])
                {
                    MainWindow.playerTurns[player].Hand.Items.Add(card);
                }
                MainWindow.playerTurns[player].Table_Money.Items.Clear();
                foreach (Card card in MainWindow.AllTableMoney[player])
                {
                    MainWindow.playerTurns[player].Table_Money.Items.Add(card);
                }
                MainWindow.playerTurns[player].Table_Properties.Items.Clear();
                foreach (List<Card> propColor in MainWindow.AllTableProperties[player])
                {
                    foreach (Card card in propColor)
                    {
                        MainWindow.playerTurns[player].Table_Properties.Items.Add(card);
                    }
                }
                for (int otherPlayer = 0; otherPlayer < MainWindow.numOfPlayers; otherPlayer++)
                {
                    MainWindow.otherTable_Money[player][otherPlayer].Items.Clear();
                    MainWindow.otherTable_Properties[player][otherPlayer].Items.Clear();
                    if (otherPlayer != player)
                    {
                        MainWindow.otherCardsLeft[player][otherPlayer].Text = MainWindow.AllHands[otherPlayer].Count().ToString();
                        if (otherPlayer == MainWindow.playerNum)
                        {
                            MainWindow.otherTurnsLeft[player][otherPlayer].Text = (3 - MainWindow.playNum).ToString();
                        }
                        else
                        {
                            MainWindow.otherTurnsLeft[player][otherPlayer].Text = "0";
                        }
                        foreach (Card card in MainWindow.AllTableMoney[otherPlayer])
                        {
                            MainWindow.otherTable_Money[player][otherPlayer].Items.Add(card);
                        }
                        foreach (List<Card> propColor in MainWindow.AllTableProperties[otherPlayer])
                        {
                            foreach (Card card in propColor)
                            {
                                MainWindow.otherTable_Properties[player][otherPlayer].Items.Add(card);
                            }
                        }
                    }
                }
            }
            //MainWindow.sendGameStates();
        }

        public void updateUniversalPrompt(string toDisplay)
        {
            MainWindow.newUniversalPrompt += Environment.NewLine + toDisplay;
            //MainWindow.sendGameStates();
            //for (int player = 0; player < MainWindow.numOfPlayers; player++)
            //{
            //    MainWindow.playerTurns[player].universalPrompt.Text += toDisplay + Environment.NewLine;
            //    MainWindow.playerTurns[player].universalPrompt.ScrollToEnd();
            //}
        }

        //Begin turn... Decision tree
        public void decidePlayType()
        {
            if (MainWindow.checkForWinner())
            {
                return;
            }

            if (MainWindow.playNum == 4)
            {
                turnsLeftDisplay.Content = 0;
                endTurn();
                return;
            }
            for (int i = 0; i < MainWindow.playerNum; i++)
            {
                //MainWindow.playerTurns[i].Hand.IsEnabled = false;
                //MainWindow.playerTurns[i].Table_Money.IsEnabled = false;
                //MainWindow.playerTurns[i].Table_Properties.IsEnabled = false;
            }
            MainWindow.stage = turnStage.decidePlayType;
            MainWindow.doublePlayed = false;
            MainWindow.doublePlayed2 = false;
            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + ", what would you like to do?";
            if (MainWindow.playNum < 3)
            {
                turnsLeftDisplay.Content = (3 - MainWindow.playNum);
                button1.Content = "Play Card From Hand";
                button1.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                turnsLeftDisplay.Content = 0;
                button1.Visibility = System.Windows.Visibility.Hidden;
            }
            button2.Content = "Move Card on Table";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Content = "End Turn";
            button3.Visibility = System.Windows.Visibility.Visible;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            //MainWindow.sendGameStates();
            this.UpdateLayout();
        }

        public void playCardFromHand()
        {
            MainWindow.stage = turnStage.playCardFromeHand;
            //Hand.IsEnabled = true;
            Prompt.Content = "Which card would you like to play?";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
            //this.UpdateLayout();
        }

        public void decideCardType()
        {
            //Hand.IsEnabled = false;
            Prompt.Content = "Play card as...";
            MainWindow.stage = turnStage.decideCardType;
            MainWindow.cardType = MainWindow.getCardType(MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum]);
            switch (MainWindow.cardType)
            {
                case CardType.Action:
                    button1.Content = "Action";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Money";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case CardType.Money:
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Content = "Money";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case CardType.Property:
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    button3.Content = "Property";
                    button3.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void playCardAsMoney()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played a " + MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum].ToString() + " as money");
            MainWindow.AllTableMoney[MainWindow.playerNum].Add(MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum]);
            MainWindow.AllHands[MainWindow.playerNum].RemoveAt(MainWindow.cardNum);
            showTable();
            MainWindow.playNum++;
            showTable();
            decidePlayType();
        }

        public void playCardAsAction()
        {
            switch (MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum])
            {
                case Card.Birthday__2:
                    checkBirthday();
                    break;

                case Card.DealBreaker__5:
                    playDealBreaker();
                    break;

                case Card.DebtCollector__3:
                    playDebtCollector();
                    break;

                case Card.ForcedDeal__3:
                    playForcedDeal();
                    break;

                case Card.PassGo__1:
                    drawCards(2);
                    updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played a Pass-Go");
                    MainWindow.AllHands[MainWindow.playerNum].Remove(Card.PassGo__1);
                    finishPlayAction();
                    break;

                case Card.SlyDeal__3:
                    playSlyDeal();
                    break;

                case Card.RentWild__3:
                    playRentWild();
                    break;

                case Card.RentBlackUtility__1:
                    MainWindow.rentCard = Card.RentBlackUtility__1;
                    playRent();
                    break;

                case Card.RentBrownLightBlue__1:
                    MainWindow.rentCard = Card.RentBrownLightBlue__1;
                    playRent();
                    break;

                case Card.RentGreenBlue__1:
                    MainWindow.rentCard = Card.RentGreenBlue__1;
                    playRent();
                    break;

                case Card.RentPinkOrange__1:
                    MainWindow.rentCard = Card.RentPinkOrange__1;
                    playRent();
                    break;

                case Card.RentRedYellow__1:
                    MainWindow.rentCard = Card.RentRedYellow__1;
                    playRent();
                    break;

                case Card.House__3:
                    playHouse();
                    break;

                case Card.Hotel__4:
                    playHotel();
                    break;
            }
        }

        public void finishPlayAction()
        {
            MainWindow.justSayNos = 0;
            MainWindow.checkProperties();
            showTable();
            setupEvents(MainWindow.playerNum);
            if (MainWindow.payersLeft.Count() == 0)
            {
                MainWindow.playNum++;
                if (MainWindow.doublePlayed)
                {
                    MainWindow.playNum++;
                }
                if (MainWindow.doublePlayed2)
                {
                    MainWindow.playNum++;
                }
                showTable();
                decidePlayType();
            }
            else //Still waiting for other players to pay
            {
                Prompt.Content = "Waiting for: ";
                foreach (int player in MainWindow.payersLeft)
                {
                    Prompt.Content += MainWindow.playerNames[player] + ", ";
                }
                MainWindow.chosenPlayer = MainWindow.payersLeft[0];
                MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
            }
            //MainWindow.sendGameStates();

        }

        //Playing card as property
        public void decidePropertyType()
        {
            MainWindow.stage = turnStage.decidePropertyType;
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            Prompt.Content = "Which color would you like to play this as?";
            switch (MainWindow.propertyCard)
            {
                case Card.PropertyBlackGreen__4:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Green";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyBlackLightBlue__4:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Light Blue";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyBlackUtility__2:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Utilities";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyBlueGreen__4:
                    button1.Content = "Blue";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Green";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyBrownLightBlue__1:
                    button1.Content = "Brown";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Light Blue";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyGreenBlack__4:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Green";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyGreenBlue__4:
                    button1.Content = "Blue";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Green";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyLightBlueBlack__4:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Light Blue";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyLightBlueBrown__1:
                    button1.Content = "Brown";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Light Blue";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyUtilityBlack__2:
                    button1.Content = "Black";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Utilities";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyOrangePink__2:
                    button1.Content = "Orange";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Pink";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyPinkOrange__2:
                    button1.Content = "Orange";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Pink";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyRedYellow__3:
                    button1.Content = "Red";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Yellow";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
                case Card.PropertyYellowRed__3:
                    button1.Content = "Red";
                    button1.Visibility = System.Windows.Visibility.Visible;
                    button2.Content = "Yellow";
                    button2.Visibility = System.Windows.Visibility.Visible;
                    return;
            }
            //MainWindow.sendGameStates();

        }

        public void decidePropertyTypeWild()
        {
            MainWindow.stage = turnStage.decidePropertyTypeWild;
            button1.Content = "Play as Wild";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            Prompt.Content = "Select property to play Wild as.";
            //Table_Properties.IsEnabled = true;
            Table_Properties.SelectionMode = SelectionMode.Single;
            //MainWindow.sendGameStates();
        }

        public void playCardAsProperty()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played a " + MainWindow.propertyCard.ToString());
            if (MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex])
            {
                MainWindow.AllTableProperties[MainWindow.playerNum][10].Add(MainWindow.propertyCard);
            }
            else
            {
                MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(MainWindow.propertyCard);
            }
            MainWindow.AllHands[MainWindow.playerNum].RemoveAt(MainWindow.cardNum);
            MainWindow.playNum++;
            showTable();
            decidePlayType();
        }

        //Check all plays from hand
        public void checkPlayCard()
        {
            if (MainWindow.stage != turnStage.decidePropertyTypeWild)
            {
                MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
            }
            if ((MainWindow.stage == turnStage.decidePropertyType) || (MainWindow.stage == turnStage.decidePropertyTypeWild))
            {
                string color = "";
                switch (MainWindow.propIndex)
                {
                    case 0:
                        color = "Brown";
                        break;
                    case 1:
                        color = "Utility";
                        break;
                    case 2:
                        color = "Blue";
                        break;
                    case 3:
                        color = "Light Blue";
                        break;
                    case 4:
                        color = "Pink";
                        break;
                    case 5:
                        color = "Orange";
                        break;
                    case 6:
                        color = "Red";
                        break;
                    case 7:
                        color = "Yellow";
                        break;
                    case 8:
                        color = "Green";
                        break;
                    case 9:
                        color = "Black";
                        break;
                    case 10:
                        color = "Wild";
                        break;
                }
                Prompt.Content = "Play " + MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum] + " as " + color + " " + MainWindow.cardType + "?";
            }
            else
            {
                Prompt.Content = "Play " + MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum] + " as " + MainWindow.cardType + "?";
            }
            //Hand.IsEnabled = false;
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            MainWindow.stage = turnStage.checkPlayCard;
            //MainWindow.sendGameStates();
        }

        //Moving cards on table
        public void moveCards()
        {
            MainWindow.stage = turnStage.moveCards;
            Prompt.Content = "Select card to move.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //Table_Properties.IsEnabled = true;
            Table_Properties.SelectionMode = SelectionMode.Single;
            //MainWindow.sendGameStates();
        }

        public void moveCardsDecideType()
        {
            if (MainWindow.hasBeenRented[MainWindow.playerNum][MainWindow.propIndex])
            {
                Prompt.Content = "You can't move a property after renting it.  Nice try, asshole.";
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                MainWindow.stage = turnStage.moveCardsDecideType;
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Visible;
                Prompt.Content = "Which color would you like to play this as?";
                switch (MainWindow.propertyCard)
                {
                    case Card.PropertyBlackGreen__4:
                        button1.Content = "Green";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlackLightBlue__4:
                        button1.Content = "Light Blue";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlackUtility__2:
                        button1.Content = "Utilities";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlueGreen__4:
                        button1.Content = "Green";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBrownLightBlue__1:
                        button1.Content = "Light Blue";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyGreenBlack__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyGreenBlue__4:
                        button1.Content = "Blue";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyLightBlueBlack__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyLightBlueBrown__1:
                        button1.Content = "Brown";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyUtilityBlack__2:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyOrangePink__2:
                        button1.Content = "Pink";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyPinkOrange__2:
                        button1.Content = "Orange";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyRedYellow__3:
                        button1.Content = "Yellow";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyYellowRed__3:
                        button1.Content = "Red";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        return;
                }
            }
            //MainWindow.sendGameStates();
        }

        public void moveCardsDecideTypeWild()
        {
            if (MainWindow.hasBeenRented[MainWindow.playerNum][MainWindow.propIndex])
            {
                Prompt.Content = "You can't move a property after renting it.  Nice try, asshole.";
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                MainWindow.stage = turnStage.moveCardsDecideTypeWild;
                button1.Content = "Play as Wild";
                button1.Visibility = System.Windows.Visibility.Visible;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Visible;
                Prompt.Content = "Select property to play Wild as.";
                //Table_Properties.IsEnabled = true;
                Table_Properties.SelectionMode = SelectionMode.Single;
            }
            //MainWindow.sendGameStates();
        }

        public void checkMoveCard()
        {
            //if ((MainWindow.stage == turnStage.moveCardsDecideType) || (MainWindow.stage == turnStage.moveCardsDecideTypeWild))
            //{
            MainWindow.stage = turnStage.checkMoveCard;
            string color = "";
            switch (MainWindow.propIndex2)
            {
                case 0:
                    color = "Brown";
                    break;
                case 1:
                    color = "Utility";
                    break;
                case 2:
                    color = "Blue";
                    break;
                case 3:
                    color = "Light Blue";
                    break;
                case 4:
                    color = "Pink";
                    break;
                case 5:
                    color = "Orange";
                    break;
                case 6:
                    color = "Red";
                    break;
                case 7:
                    color = "Yellow";
                    break;
                case 8:
                    color = "Green";
                    break;
                case 9:
                    color = "Black";
                    break;
                case 10:
                    color = "Wild";
                    break;
            }
            if (MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex2])
            {
                foreach (Card card in MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2])
                {
                    if (MainWindow.getPropertyType(card) == PropertyType.Normal)
                    {
                        Prompt.Content = "Switch " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum] + " for a " + color + " property?";
                        //Hand.IsEnabled = false;
                        button1.Content = "Yes";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "No";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        button3.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                        return;
                    }
                    else
                    {
                        Prompt.Content = "That monopoly is already full... Move a card from it first.";
                        //Hand.IsEnabled = false;
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        button3.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            Prompt.Content = "Play " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum] + " as " + color + " " + MainWindow.cardType + "?";
            //}
            //else
            //{
            //    Prompt.Content = "Play " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum] + " as " + MainWindow.cardType + "?";
            //}
            //Hand.IsEnabled = false;
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void finishMoveCard()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " moved a " + MainWindow.propertyCard.ToString());
            if (MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex2])
            {
                foreach (Card card in MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2])
                {
                    if (MainWindow.getPropertyType(card) == PropertyType.Normal)
                    {
                        MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2].Remove(card);
                        MainWindow.checkProperties();
                        MainWindow.AllTableProperties[MainWindow.playerNum][10].Add(card);
                        break;
                    }
                }
            }
            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2].Add(MainWindow.propertyCard);
            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].RemoveAt(MainWindow.cardNum);
            showTable();
            decidePlayType();
        }

        //End turn
        public void discard()
        {
            MainWindow.stage = turnStage.discard;
            //Hand.IsEnabled = true;
            Prompt.Content = "Choose a card to discard";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            if (MainWindow.playNum < 3)
            {
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                buttonBack.Visibility = System.Windows.Visibility.Hidden;
            }
            //MainWindow.sendGameStates();
        }

        public void checkDiscard()
        {
            MainWindow.stage = turnStage.checkDiscard;
            //Hand.IsEnabled = false;
            Prompt.Content = "Discard " + MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum] + " as " + MainWindow.cardType + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void discardCard()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " discarded a " + MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum].ToString());
            MainWindow.playNum = 4;
            MainWindow.deckDiscard.Add(MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum]);
            MainWindow.AllHands[MainWindow.playerNum].RemoveAt(MainWindow.cardNum);
            showTable();
            decidePlayType();
        }

        public void endTurn()
        {
            turnsLeftDisplay.Content = 0;
            for (int i = 0; i < 11; i++)
            {
                MainWindow.hasBeenRented[MainWindow.playerNum][i] = false;
            }
            if (MainWindow.AllHands[MainWindow.playerNum].Count() > 7)
            {
                discard();
            }
            else
            {
                updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " ended his turn");
                //MainWindow.playerTurns[MainWindow.playerNum].window.WindowState = System.Windows.WindowState.Minimized;
                MainWindow.playerNum++;
                if (MainWindow.playerNum == MainWindow.numOfPlayers)
                {
                    MainWindow.playerNum = 0;
                }
                for (int player = 0; player < MainWindow.numOfPlayers; player++)
                {
                    MainWindow.playerTurns[player].Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";
                }
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Hidden;
                //MainWindow.playerTurns[MainWindow.playerNum].beginTurn(MainWindow.playerNum);
                MainWindow.playerTurns[MainWindow.playerNum].readyToBegin();
            }
        }

        public void readyToBegin()
        {
            //MainWindow.playerTurns[MainWindow.playerNum].window.WindowState = System.Windows.WindowState.Maximized;
            //MainWindow.playerTurns[MainWindow.playerNum].window.BringIntoView();
            //MainWindow.playerTurns[MainWindow.playerNum].window.Focus();
            //MainWindow.playerTurns[MainWindow.playerNum].window.Show();
            MainWindow.stage = turnStage.ready;
            //for (int i = 0; i < MainWindow.playerNum; i++)
            //{
            //    MainWindow.playerTurns[i].Hand.Visibility = System.Windows.Visibility.Hidden;
            //}
            button1.Content = "Begin Turn";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            //MainWindow.sendGameStates();
        }

        //House
        public void playHouse()
        {
            MainWindow.stage = turnStage.house;
            int numOfOptions = MainWindow.numOfHouseOptions();
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            if (numOfOptions == 0)
            {
                Prompt.Content = "You have no Monopolies to add a House to...";
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            if (numOfOptions > 0)
            {
                Prompt.Content = "Select a property set to add a house to";
                for (int index = 0; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][index].Count == MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex1 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button1.Content = color;
                        button1.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            if (numOfOptions == 2)
            {
                for (int index = MainWindow.monopolyIndex1; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][index].Count == MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex2 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button2.Content = color;
                        button2.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            //MainWindow.sendGameStates();
        }

        public void checkHouse()
        {
            MainWindow.stage = turnStage.checkHouse;
            string color = " ";
            switch (MainWindow.propIndex)
            {
                case 0:
                    color = "Brown";
                    break;
                case 1:
                    color = "Utility";
                    break;
                case 2:
                    color = "Blue";
                    break;
                case 3:
                    color = "Light Blue";
                    break;
                case 4:
                    color = "Pink";
                    break;
                case 5:
                    color = "Orange";
                    break;
                case 6:
                    color = "Red";
                    break;
                case 7:
                    color = "Yellow";
                    break;
                case 8:
                    color = "Green";
                    break;
                case 9:
                    color = "Black";
                    break;
            }
            MainWindow.propColor = color;
            Prompt.Content = "Play House on " + color + " properties?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void house()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played a House on his " + MainWindow.propColor + " properties");
            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(Card.House__3);
            MainWindow.AllHands[MainWindow.playerNum].RemoveAt(MainWindow.cardNum);
            showTable();
            MainWindow.playNum++;
            decidePlayType();
        }

        //House
        public void playHotel()
        {
            MainWindow.stage = turnStage.hotel;
            int numOfOptions = MainWindow.numOfHotelOptions();
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            if (numOfOptions == 0)
            {
                Prompt.Content = "You have no Monopolies to add a Hotel to...";
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            if (numOfOptions > 0)
            {
                Prompt.Content = "Select a property set to add a hotel to";
                for (int index = 0; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][index].Count == MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex1 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button1.Content = color;
                        button1.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            if (numOfOptions == 2)
            {
                for (int index = MainWindow.monopolyIndex1; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][index].Count == MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex2 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button2.Content = color;
                        button2.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            //MainWindow.sendGameStates();
        }

        public void checkHotel()
        {
            MainWindow.stage = turnStage.checkHotel;
            string color = " ";
            switch (MainWindow.propIndex)
            {
                case 0:
                    color = "Brown";
                    break;
                case 1:
                    color = "Utility";
                    break;
                case 2:
                    color = "Blue";
                    break;
                case 3:
                    color = "Light Blue";
                    break;
                case 4:
                    color = "Pink";
                    break;
                case 5:
                    color = "Orange";
                    break;
                case 6:
                    color = "Red";
                    break;
                case 7:
                    color = "Yellow";
                    break;
                case 8:
                    color = "Green";
                    break;
                case 9:
                    color = "Black";
                    break;
            }
            MainWindow.propColor = color;
            Prompt.Content = "Play Hotel on " + color + " properties?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void hotel()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played a Hotel on his " + MainWindow.propColor + " properties");
            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(Card.House__3);
            MainWindow.AllHands[MainWindow.playerNum].RemoveAt(MainWindow.cardNum);
            showTable();
            MainWindow.playNum++;
            decidePlayType();
        }

        //Birthday
        public void checkBirthday()
        {
            MainWindow.stage = turnStage.birthday;
            Prompt.Content = "Play 'It's My Birthday'? ";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void collectBirthday()
        {
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            if ((month == 1) && (day == 12) && (MainWindow.playerNames[MainWindow.playerNum] != "Eric"))
            {
                updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played It's My Birthday!  Even though it's Eric's birthday... Kinda messed up really. Dirty filthy liar...");
            }
            else
            {
                updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " played It's My Birthday!");
            }
            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.Birthday__2);
            MainWindow.playedCard = Card.Birthday__2;
            showTable();
            for (int i = (MainWindow.playerNum + 1); i < MainWindow.numOfPlayers; i++)
            {
                MainWindow.payersLeft.Add(i);
            }
            for (int i = 0; i < MainWindow.playerNum; i++)
            {
                MainWindow.payersLeft.Add(i);
            }
            Prompt.Content = "Waiting for: ";
            foreach (int player in MainWindow.payersLeft)
            {
                Prompt.Content += MainWindow.playerNames[player] + ", ";
            }
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.payment = 2;
            MainWindow.chosenPlayer = MainWindow.payersLeft[0];
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //Debt Collect
        public void playDebtCollector()
        {
            MainWindow.stage = turnStage.debtCollect;
            Prompt.Content = "Choose a player to Debt Collect.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void checkDebtCollector()
        {
            Prompt.Content = "Debt Collect " + MainWindow.playerNames[MainWindow.chosenPlayer] + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void debtCollect()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " Debt Collected " + MainWindow.playerNames[MainWindow.chosenPlayer]);
            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.DebtCollector__3);
            MainWindow.playedCard = Card.DebtCollector__3;
            showTable();
            Prompt.Content = "Waiting for " + (MainWindow.playerNames[MainWindow.chosenPlayer]) + " to select payment...";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.payment = 5;
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //Rent
        public void playRentWild()
        {
            MainWindow.stage = turnStage.rentWild;
            Prompt.Content = "Choose a property to rent.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.IsEnabled = true;
            MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.SelectionMode = SelectionMode.Single;
            //MainWindow.sendGameStates();
        }

        public void playRentWild2()
        {
            MainWindow.stage = turnStage.rentWild2;
            Prompt.Content = "Choose a player to charge $" + MainWindow.payment + " rent.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.IsEnabled = false;
            MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.SelectionMode = SelectionMode.Single;
            //MainWindow.sendGameStates();
        }

        public void checkRentWild()
        {
            Prompt.Content = "Charge " + MainWindow.playerNames[MainWindow.chosenPlayer] + " $" + MainWindow.payment + " in rent?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void rentWild()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " charged " + MainWindow.playerNames[MainWindow.chosenPlayer] + " $" + MainWindow.payment + " in rent");
            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.RentWild__3);
            if (MainWindow.doublePlayed)
            {
                MainWindow.AllHands[MainWindow.playerNum].Remove(Card.DoubleTheRent__1);
            }
            if (MainWindow.doublePlayed2)
            {
                MainWindow.AllHands[MainWindow.playerNum].Remove(Card.DoubleTheRent__1);
            }
            if (MainWindow.doublePlayed2)
            {
                updateUniversalPrompt("(DOUBLED the doubled rent...)");
            }
            else if (MainWindow.doublePlayed)
            {
                updateUniversalPrompt("(Doubled the rent...)");
            }
            MainWindow.playedCard = Card.RentWild__3;
            MainWindow.hasBeenRented[MainWindow.playerNum][MainWindow.propIndex] = true;
            showTable();
            Prompt.Content = "Waiting for " + (MainWindow.playerNames[MainWindow.chosenPlayer]) + " to select payment...";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        public void playRent()
        {
            MainWindow.stage = turnStage.rent;
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            Prompt.Content = "Choose a property to rent.";
            switch (MainWindow.rentCard)
            {
                case Card.RentBlackUtility__1:
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][9].Count > 0)
                    {
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][1].Count > 0)
                    {
                        button2.Content = "Utilities";
                        button2.Visibility = System.Windows.Visibility.Visible;
                    }
                    if ((MainWindow.AllTableProperties[MainWindow.playerNum][9].Count == 0) && (MainWindow.AllTableProperties[MainWindow.playerNum][1].Count == 0))
                    {
                        Prompt.Content = "You have no properties to rent with this card.";
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                    return;

                case Card.RentBrownLightBlue__1:
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][0].Count > 0)
                    {
                        button1.Content = "Brown";
                        button1.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][3].Count > 0)
                    {
                        button2.Content = "Light Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                    }
                    if ((MainWindow.AllTableProperties[MainWindow.playerNum][0].Count == 0) && (MainWindow.AllTableProperties[MainWindow.playerNum][3].Count == 0))
                    {
                        Prompt.Content = "You have no properties to rent with this card.";
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                    return;

                case Card.RentGreenBlue__1:
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][8].Count > 0)
                    {
                        button1.Content = "Green";
                        button1.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][2].Count > 0)
                    {
                        button2.Content = "Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                    }
                    if ((MainWindow.AllTableProperties[MainWindow.playerNum][8].Count == 0) && (MainWindow.AllTableProperties[MainWindow.playerNum][2].Count == 0))
                    {
                        Prompt.Content = "You have no properties to rent with this card.";
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                    return;

                case Card.RentPinkOrange__1:
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][4].Count > 0)
                    {
                        button1.Content = "Pink";
                        button1.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][5].Count > 0)
                    {
                        button2.Content = "Orange";
                        button2.Visibility = System.Windows.Visibility.Visible;
                    }
                    if ((MainWindow.AllTableProperties[MainWindow.playerNum][4].Count == 0) && (MainWindow.AllTableProperties[MainWindow.playerNum][5].Count == 0))
                    {
                        Prompt.Content = "You have no properties to rent with this card.";
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                    return;

                case Card.RentRedYellow__1:
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][6].Count > 0)
                    {
                        button1.Content = "Red";
                        button1.Visibility = System.Windows.Visibility.Visible;
                    }
                    if (MainWindow.AllTableProperties[MainWindow.playerNum][7].Count > 0)
                    {
                        button2.Content = "Yellow";
                        button2.Visibility = System.Windows.Visibility.Visible;
                    }
                    if ((MainWindow.AllTableProperties[MainWindow.playerNum][6].Count == 0) && (MainWindow.AllTableProperties[MainWindow.playerNum][7].Count == 0))
                    {
                        Prompt.Content = "You have no properties to rent with this card.";
                        button1.Visibility = System.Windows.Visibility.Hidden;
                        button2.Visibility = System.Windows.Visibility.Hidden;
                        buttonBack.Visibility = System.Windows.Visibility.Visible;
                    }
                    return;
            }
            //MainWindow.sendGameStates();
        }

        public void checkRent()
        {
            MainWindow.stage = turnStage.checkRent;
            //MainWindow.payment = MainWindow.getRentAmount();
            Prompt.Content = "Charge $" + MainWindow.payment + " rent to everyone?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void rent()
        {
            if (MainWindow.payment == 60)
            {
                updateUniversalPrompt("I don't believe it... Either I glitched, or IT happened...");
            }
            else if (MainWindow.payment >= 20)
            {
                updateUniversalPrompt("Hot damn...");
            }
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " charged everyone $" + MainWindow.payment + " in rent");
            if (MainWindow.doublePlayed2)
            {
                updateUniversalPrompt("(DOUBLED the doubled rent...)");
            }
            else if (MainWindow.doublePlayed)
            {
                updateUniversalPrompt("(Doubled the rent...)");
            }
            MainWindow.AllHands[MainWindow.playerNum].Remove(MainWindow.rentCard);
            MainWindow.playedCard = Card.Rent;
            MainWindow.hasBeenRented[MainWindow.playerNum][MainWindow.propIndex] = true;
            showTable();
            for (int i = (MainWindow.playerNum + 1); i < MainWindow.numOfPlayers; i++)
            {
                MainWindow.payersLeft.Add(i);
            }
            for (int i = 0; i < MainWindow.playerNum; i++)
            {
                MainWindow.payersLeft.Add(i);
            }
            Prompt.Content = "Waiting for: ";
            foreach (int player in MainWindow.payersLeft)
            {
                Prompt.Content += MainWindow.playerNames[player] + ", ";
            }
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.chosenPlayer = MainWindow.payersLeft[0];
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //DoubleTheRent
        public void askDoubleRentWild()
        {
            MainWindow.stage = turnStage.doubleRentWild;
            Prompt.Content = "Would you like to Double the Rent to $" + (MainWindow.payment * 2) + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void askDoubleRent()
        {
            MainWindow.stage = turnStage.doubleRent;
            Prompt.Content = "Would you like to Double the Rent to $" + (MainWindow.payment * 2) + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        //Forced Deal
        public void playForcedDeal()
        {
            MainWindow.stage = turnStage.forcedDeal1;
            Prompt.Content = "Choose a card from your table to trade.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.IsEnabled = true;
            MainWindow.playerTurns[MainWindow.playerNum].Table_Properties.SelectionMode = SelectionMode.Single;
            //MainWindow.sendGameStates();
        }

        public void playForcedDeal2()
        {
            MainWindow.stage = turnStage.forcedDeal2;
            Prompt.Content = "Choose a card from another player's table to steal.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                if (i != MainWindow.playerNum)
                {
                    //MainWindow.playerTurns[i].Table_Properties.IsEnabled = true;
                    MainWindow.playerTurns[i].Table_Properties.SelectionMode = SelectionMode.Single;
                }
            }
            //MainWindow.sendGameStates();
        }

        public void checkForcedDeal()
        {
            MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.SelectedItem = null;
            //MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.IsEnabled = false;
            MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.SelectionMode = SelectionMode.Single;
            if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex2])
            {
                if (!(MainWindow.AllTableProperties[MainWindow.chosenPlayer][10].Contains(MainWindow.propertyCard)))
                {
                    Prompt.Content = "You can't steal from a monopoly...";
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    buttonBack.Visibility = System.Windows.Visibility.Visible;
                    return;
                }
                else
                {
                    MainWindow.propIndex2 = 10;
                    MainWindow.cardNum2 = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2].IndexOf(MainWindow.propertyCard);
                }
            }
            Prompt.Content = "Trade your " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum] + " for " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2][MainWindow.cardNum2] + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                //MainWindow.playerTurns[i].Table_Properties.IsEnabled = false;
            }
            //MainWindow.sendGameStates();
        }

        public void forcedDeal()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " Forced-Dealing his " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum] + " for " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2][MainWindow.cardNum2]);
            MainWindow.playedCard = Card.ForcedDeal__3;
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //Sly Deal
        public void playSlyDeal()
        {
            MainWindow.stage = turnStage.slyDeal;
            Prompt.Content = "Choose a card to Sly Deal.";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                if (i != MainWindow.playerNum)
                {
                    //MainWindow.playerTurns[i].Table_Properties.IsEnabled = true;
                    MainWindow.playerTurns[i].Table_Properties.SelectionMode = SelectionMode.Single;
                }
            }
            //MainWindow.sendGameStates();
        }

        public void checkSlyDeal()
        {
            MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.SelectedItem = null;
            //MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.IsEnabled = false;
            MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.SelectionMode = SelectionMode.Single;
            if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex])
            {
                if (!(MainWindow.AllTableProperties[MainWindow.chosenPlayer][10].Contains(MainWindow.propertyCard)))
                {
                    Prompt.Content = "You can't steal from a monopoly...";
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    buttonBack.Visibility = System.Windows.Visibility.Visible;
                    return;
                }
                else
                {
                    MainWindow.propIndex = 10;
                    MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].IndexOf(MainWindow.propertyCard);
                }
            }
            Prompt.Content = "Sly Deal " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex][MainWindow.cardNum] + " from " + MainWindow.playerNames[MainWindow.chosenPlayer] + "?";
            button1.Content = "Yes";
            button1.Visibility = System.Windows.Visibility.Visible;
            button2.Content = "No";
            button2.Visibility = System.Windows.Visibility.Visible;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                //MainWindow.playerTurns[i].Table_Properties.IsEnabled = false;
            }
            //MainWindow.sendGameStates();
        }

        public void slyDeal()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " is Sly-Dealing " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex][MainWindow.cardNum]);
            MainWindow.playedCard = Card.SlyDeal__3;
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //Dealbreaker
        public void playDealBreaker()
        {
            MainWindow.stage = turnStage.dealBreaker;
            Prompt.Content = "Choose a player to Deal-Break";
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                if (i != MainWindow.playerNum)
                {
                    //MainWindow.playerTurns[i].Table_Properties.IsEnabled = true;
                    MainWindow.playerTurns[i].Table_Properties.SelectionMode = SelectionMode.Single;
                }
            }
            //MainWindow.sendGameStates();
        }

        public void playDealBreaker2()
        {
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Visible;
            for (int i = 0; i < MainWindow.numOfPlayers; i++)
            {
                //MainWindow.playerTurns[i].Table_Properties.IsEnabled = false;
            }
            int numOfOptions = MainWindow.numOfMonopolies();
            if (numOfOptions == 0)
            {
                Prompt.Content = MainWindow.playerNames[MainWindow.chosenPlayer] + " has no Monopolies...";
                buttonBack.Visibility = System.Windows.Visibility.Visible;
            }
            if (numOfOptions > 0)
            {
                Prompt.Content = "Choose a Monopoly to Deal-Break";
                for (int index = 0; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][index].Count >= MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex1 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button1.Content = color;
                        MainWindow.propColor = color;
                        button1.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            if (numOfOptions == 2)
            {
                for (int index = MainWindow.monopolyIndex1 + 1; index < 10; index++)
                {
                    if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][index].Count >= MainWindow.monopolyPropsNeeded[index])
                    {
                        MainWindow.monopolyIndex2 = index;
                        string color = " ";
                        switch (index)
                        {
                            case 0:
                                color = "Brown";
                                break;
                            case 1:
                                color = "Utility";
                                break;
                            case 2:
                                color = "Blue";
                                break;
                            case 3:
                                color = "Light Blue";
                                break;
                            case 4:
                                color = "Pink";
                                break;
                            case 5:
                                color = "Orange";
                                break;
                            case 6:
                                color = "Red";
                                break;
                            case 7:
                                color = "Yellow";
                                break;
                            case 8:
                                color = "Green";
                                break;
                            case 9:
                                color = "Black";
                                break;
                        }
                        button2.Content = color;
                        button2.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }
            }
            //MainWindow.sendGameStates();
        }

        public void dealBreak()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.playerNum] + " is Deal-Breaking " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s " + MainWindow.propColor + " Monopoly");
            MainWindow.playedCard = Card.DealBreaker__5;
            MainWindow.playerTurns[MainWindow.chosenPlayer].acknowledgeAttack();
        }

        //Payments
        public void acknowledgeAttack()
        {
            MainWindow.stage = turnStage.acknowledgeAttack1;
            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + " is ";
            button1.Visibility = System.Windows.Visibility.Visible;
            button1.Content = "OK";
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;

            //Hide Original player's buttons
            MainWindow.playerTurns[MainWindow.playerNum].button1.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.playerTurns[MainWindow.playerNum].button2.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.playerTurns[MainWindow.playerNum].button3.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.playerTurns[MainWindow.playerNum].buttonBack.Visibility = System.Windows.Visibility.Hidden;

            switch (MainWindow.playedCard)
            {
                case Card.Birthday__2:
                    Prompt.Content += "claiming it's his Birthday...";
                    break;

                case Card.DealBreaker__5:
                    string color = " ";
                    switch (MainWindow.monopolyIndex1)
                    {
                        case 0:
                            color = "Brown";
                            break;
                        case 1:
                            color = "Utility";
                            break;
                        case 2:
                            color = "Blue";
                            break;
                        case 3:
                            color = "Light Blue";
                            break;
                        case 4:
                            color = "Pink";
                            break;
                        case 5:
                            color = "Orange";
                            break;
                        case 6:
                            color = "Red";
                            break;
                        case 7:
                            color = "Yellow";
                            break;
                        case 8:
                            color = "Green";
                            break;
                        case 9:
                            color = "Black";
                            break;
                    }
                    Prompt.Content += "using Deal-Breaker on your " + color + " properties";
                    break;

                case Card.DebtCollector__3:
                    Prompt.Content += "Debt-Collecting you...";
                    break;

                case Card.ForcedDeal__3:
                    Prompt.Content += "Forced-Dealing your " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2][MainWindow.cardNum2] + " for a " + MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum];
                    break;

                case Card.SlyDeal__3:
                    Prompt.Content += "Sly-Dealing your " + MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex][MainWindow.cardNum];
                    break;

                case Card.RentWild__3:
                    Prompt.Content += "Charging you $" + MainWindow.payment + " rent";
                    break;

                case Card.Rent:
                    Prompt.Content += "Charging everyone $" + MainWindow.payment + " rent";
                    break;
            }
            if (MainWindow.AllHands[MainWindow.chosenPlayer].Contains(Card.JustSayNo__4))
            {
                button2.Content = "Just Say No";
                button2.Visibility = System.Windows.Visibility.Visible;
            }
            //MainWindow.sendGameStates();
        }

        public void pay()
        {
            //MainWindow.stage = turnStage.makingPayment;
            setupEvents(MainWindow.chosenPlayer);
            Prompt.Content = "Select cards totalling $" + MainWindow.payment + " to pay " + MainWindow.playerNames[MainWindow.playerNum];
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;

            int totalValue = 0;
            foreach (Card card in Table_Properties.Items)
            {
                totalValue += MainWindow.getValue(card);
            }
            foreach (Card card in Table_Money.Items)
            {
                totalValue += MainWindow.getValue(card);
            }
            if (totalValue == 0)
            {
                button3.Content = "Make Payment.";
                button3.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                //MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Money.IsEnabled = true;
                //MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.IsEnabled = true;
                MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Money.SelectionMode = SelectionMode.Multiple;
                MainWindow.playerTurns[MainWindow.chosenPlayer].Table_Properties.SelectionMode = SelectionMode.Multiple;
            }
            //MainWindow.sendGameStates();
        }

        public void justSayNo()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.chosenPlayer] + " Just Said No");
            MainWindow.AllHands[MainWindow.chosenPlayer].Remove(Card.JustSayNo__4);
            MainWindow.payersLeft.Remove(MainWindow.chosenPlayer);
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            //Table_Money.IsEnabled = false;
            //Table_Properties.IsEnabled = false;
            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";

            //Wait for original player to acknowledge...
            MainWindow.playerTurns[MainWindow.playerNum].Prompt.Content = MainWindow.playerNames[MainWindow.chosenPlayer] + " played a Just Say No";
            MainWindow.playerTurns[MainWindow.playerNum].button1.Content = "OK";
            MainWindow.playerTurns[MainWindow.playerNum].button1.Visibility = System.Windows.Visibility.Visible;
            if (MainWindow.AllHands[MainWindow.playerNum].Contains(Card.JustSayNo__4))
            {
                MainWindow.playerTurns[MainWindow.playerNum].button2.Content = "Just Say No";
                MainWindow.playerTurns[MainWindow.playerNum].button2.Visibility = System.Windows.Visibility.Visible;
            }
            //MainWindow.sendGameStates();
        }

        public void justSayNoOwner()
        {
            updateUniversalPrompt(MainWindow.playerNames[MainWindow.chosenPlayer] + " Just Said No to " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s Just Say No!");
            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.JustSayNo__4);
            MainWindow.playerTurns[MainWindow.chosenPlayer].Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "Just Said No to your Just Say No.  Get rocked...";
            MainWindow.playerTurns[MainWindow.chosenPlayer].button1.Content = "OK";
            MainWindow.playerTurns[MainWindow.chosenPlayer].button1.Visibility = System.Windows.Visibility.Visible;
            MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Visibility = System.Windows.Visibility.Hidden;
            if (MainWindow.AllHands[MainWindow.chosenPlayer].Contains(Card.JustSayNo__4))
            {
                MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Content = "Just Say No";
                MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Visibility = System.Windows.Visibility.Visible;
            }
            MainWindow.playerTurns[MainWindow.chosenPlayer].button3.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.playerTurns[MainWindow.chosenPlayer].buttonBack.Visibility = System.Windows.Visibility.Hidden;
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            switch (MainWindow.playedCard)
            {
                case Card.Birthday__2:
                    MainWindow.payersLeft.Add(MainWindow.chosenPlayer);
                    foreach (int i in MainWindow.payersLeft)
                    {
                        Prompt.Content += MainWindow.playerNames[i] + ", ";
                    }
                    break;

                case Card.Rent:
                    foreach (int i in MainWindow.payersLeft)
                    {
                        Prompt.Content += MainWindow.playerNames[i] + ", ";
                    }
                    break;
            }
            //MainWindow.sendGameStates();
            return;
        }

        public void justSayNoAgain()
        {
            updateUniversalPrompt("GET ROCKED!!! " + MainWindow.playerNames[MainWindow.chosenPlayer] + " Just Said No to " + MainWindow.playerNames[MainWindow.playerNum] + " Just Saying No to " + MainWindow.playerNames[MainWindow.chosenPlayer] + "'s Just Say No!!!!");
            MainWindow.AllHands[MainWindow.chosenPlayer].Remove(Card.JustSayNo__4);
            MainWindow.payersLeft.Remove(MainWindow.chosenPlayer);
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            button3.Visibility = System.Windows.Visibility.Hidden;
            buttonBack.Visibility = System.Windows.Visibility.Hidden;
            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";

            //Wait for original player to acknowledge...
            MainWindow.playerTurns[MainWindow.playerNum].Prompt.Content = MainWindow.playerNames[MainWindow.chosenPlayer] + "OOOOoooooooo!  It just happened!  He Just Said No, to your Just Say No, of his Just Say No!";
            MainWindow.playerTurns[MainWindow.playerNum].button1.Content = "OK";
            MainWindow.playerTurns[MainWindow.playerNum].button1.Visibility = System.Windows.Visibility.Visible;
            //MainWindow.sendGameStates();
        }

        public void receiveForcedDeal()
        {
            MainWindow.stage = turnStage.receiveForcedDeal;
            MainWindow.propertyType = MainWindow.getPropertyType(MainWindow.propertyCard);
            if (MainWindow.propertyType == PropertyType.Wild)
            {
                button3.Content = "Play as Wild";
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Visible;
                buttonBack.Visibility = System.Windows.Visibility.Hidden;
                Prompt.Content = "Select property to play Wild as.";
                //Table_Properties.IsEnabled = true;
                Table_Properties.SelectionMode = SelectionMode.Single;
            }
            else if (MainWindow.propertyType == PropertyType.Duo)
            {
                button1.Visibility = System.Windows.Visibility.Hidden;
                button2.Visibility = System.Windows.Visibility.Hidden;
                button3.Visibility = System.Windows.Visibility.Hidden;
                buttonBack.Visibility = System.Windows.Visibility.Hidden;
                Prompt.Content = "Which color would you like to play this as?";
                switch (MainWindow.propertyCard)
                {
                    case Card.PropertyBlackGreen__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Green";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlackLightBlue__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Light Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlackUtility__2:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Utilities";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBlueGreen__4:
                        button1.Content = "Blue";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Green";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyBrownLightBlue__1:
                        button1.Content = "Brown";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Light Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyGreenBlack__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Green";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyGreenBlue__4:
                        button1.Content = "Blue";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Green";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyLightBlueBlack__4:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Light Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyLightBlueBrown__1:
                        button1.Content = "Brown";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Light Blue";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyUtilityBlack__2:
                        button1.Content = "Black";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Utilities";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyOrangePink__2:
                        button1.Content = "Orange";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Pink";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyPinkOrange__2:
                        button1.Content = "Orange";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Pink";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyRedYellow__3:
                        button1.Content = "Red";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Yellow";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                    case Card.PropertyYellowRed__3:
                        button1.Content = "Red";
                        button1.Visibility = System.Windows.Visibility.Visible;
                        button2.Content = "Yellow";
                        button2.Visibility = System.Windows.Visibility.Visible;
                        return;
                }
            }
            else //Normal
            {
                MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Add(MainWindow.propertyCard);
                MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
            }
            //MainWindow.sendGameStates();
        }

        //Events triggered by user
        #region Events from Window
        //fix acknowledge
        //public void OtherPlayer_Click(object sender, RoutedEventArgs e)
        //{
        //    Button copySender = (Button)sender;
        //    int playerClicked = MainWindow.otherNames[MainWindow.playerNum].IndexOf(copySender);

        //    if (MainWindow.stage == turnStage.debtCollect)
        //    {
        //        MainWindow.chosenPlayer = playerClicked;
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkDebtCollector();
        //        }
        //    }
        //    if (MainWindow.stage == turnStage.rentWild2)
        //    {
        //        MainWindow.chosenPlayer = playerClicked;
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkRentWild();
        //        }
        //    }
        //    if (MainWindow.stage == turnStage.dealBreaker)
        //    {
        //        MainWindow.chosenPlayer = playerClicked;
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].playDealBreaker2();
        //        }
        //    }
        //}

        //public void OtherPlayer_Properties_MouseDoubleClick(object sender, RoutedEventArgs e)
        //{
        //    ListBox copySender = (ListBox)sender;
        //    int playerClicked = MainWindow.otherTable_Properties[MainWindow.playerNum].IndexOf(copySender);

        //    if (Table_Properties.SelectedIndex < 0)
        //    {
        //        return;
        //    }
        //    if (MainWindow.stage == turnStage.slyDeal)
        //    {
        //        MainWindow.cardNum = MainWindow.otherTable_Properties[MainWindow.playerNum][playerClicked].SelectedIndex;
        //        MainWindow.propertyCard = MainWindow.otherTable_Properties[MainWindow.playerNum][playerClicked].Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //        //string chosenPlayer = buttonPlayer.Content.ToString();
        //        MainWindow.chosenPlayer = playerClicked;
        //        MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //        MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].IndexOf(MainWindow.propertyCard);
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkSlyDeal();
        //        }
        //    }

        //    //if (MainWindow.stage == turnStage.forcedDeal1)
        //    //{
        //    //    MainWindow.cardNum = Table_Properties.SelectedIndex;
        //    //    Card currentCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //    //    MainWindow.propIndex = MainWindow.getPropertyIndex(currentCard);
        //    //    MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].IndexOf(currentCard);
        //    //    MainWindow.playerTurns[MainWindow.playerNum].playForcedDeal2();
        //    //}
        //    else if (MainWindow.stage == turnStage.forcedDeal2)
        //    {
        //        MainWindow.cardNum2 = MainWindow.otherTable_Properties[MainWindow.playerNum][playerClicked].SelectedIndex;
        //        MainWindow.propertyCard = MainWindow.otherTable_Properties[MainWindow.playerNum][playerClicked].Items.Cast<Card>().ElementAt(MainWindow.cardNum2);
        //        MainWindow.propIndex2 = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //        //string chosenPlayer = buttonPlayer.Content.ToString();
        //        MainWindow.chosenPlayer = playerClicked;
        //        MainWindow.cardNum2 = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2].IndexOf(MainWindow.propertyCard);
        //        MainWindow.playerTurns[MainWindow.playerNum].checkForcedDeal();
        //    }
        //}

        public void button1_Click(object sender = null, RoutedEventArgs e = null)
        {
            switch (MainWindow.stage)
            {
                case turnStage.ready:
                    beginTurn(MainWindow.playerNum);
                    return;

                case turnStage.begin:
                    return;

                case turnStage.decidePlayType:
                    playCardFromHand();
                    return;

                case turnStage.playCardFromeHand:
                    decidePlayType();
                    return;

                case turnStage.decideCardType:
                    MainWindow.cardType = CardType.Action;
                    checkPlayCard();
                    return;

                #region decidePropertyType
                case turnStage.decidePropertyType:
                    switch (MainWindow.propertyCard)
                    {
                        case Card.PropertyBlackGreen__4:
                            MainWindow.propertyCard = Card.PropertyBlackGreen__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlackLightBlue__4:
                            MainWindow.propertyCard = Card.PropertyBlackLightBlue__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlackUtility__2:
                            MainWindow.propertyCard = Card.PropertyBlackUtility__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlueGreen__4:
                            MainWindow.propertyCard = Card.PropertyBlueGreen__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBrownLightBlue__1:
                            MainWindow.propertyCard = Card.PropertyBrownLightBlue__1;
                            checkPlayCard();
                            return;
                        case Card.PropertyGreenBlack__4:
                            MainWindow.propertyCard = Card.PropertyBlackGreen__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyGreenBlue__4:
                            MainWindow.propertyCard = Card.PropertyBlueGreen__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyLightBlueBlack__4:
                            MainWindow.propertyCard = Card.PropertyBlackLightBlue__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyLightBlueBrown__1:
                            MainWindow.propertyCard = Card.PropertyBrownLightBlue__1;
                            checkPlayCard();
                            return;
                        case Card.PropertyUtilityBlack__2:
                            MainWindow.propertyCard = Card.PropertyBlackUtility__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyOrangePink__2:
                            MainWindow.propertyCard = Card.PropertyOrangePink__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyPinkOrange__2:
                            MainWindow.propertyCard = Card.PropertyOrangePink__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyRedYellow__3:
                            MainWindow.propertyCard = Card.PropertyRedYellow__3;
                            checkPlayCard();
                            return;
                        case Card.PropertyYellowRed__3:
                            MainWindow.propertyCard = Card.PropertyRedYellow__3;
                            checkPlayCard();
                            return;
                    }
                    return;
                #endregion

                case turnStage.decidePropertyTypeWild:
                    MainWindow.propIndex = 10;
                    checkPlayCard();
                    return;

                #region checkPlayCard
                case turnStage.checkPlayCard:
                    switch (MainWindow.cardType)
                    {
                        case CardType.Action:
                            playCardAsAction();
                            break;
                        case CardType.Money:
                            playCardAsMoney();
                            break;
                        case CardType.Property:
                            MainWindow.propertyType = MainWindow.getPropertyType(MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum]);
                            if (MainWindow.propertyType == PropertyType.Wild)
                            {
                                switch (MainWindow.propIndex)
                                {
                                    case 0:
                                        MainWindow.propertyCard = Card.PropertyWildBrown;
                                        break;
                                    case 1:
                                        MainWindow.propertyCard = Card.PropertyWildUtility;
                                        break;
                                    case 2:
                                        MainWindow.propertyCard = Card.PropertyWildBlue;
                                        break;
                                    case 3:
                                        MainWindow.propertyCard = Card.PropertyWildLightBlue;
                                        break;
                                    case 4:
                                        MainWindow.propertyCard = Card.PropertyWildPink;
                                        break;
                                    case 5:
                                        MainWindow.propertyCard = Card.PropertyWildOrange;
                                        break;
                                    case 6:
                                        MainWindow.propertyCard = Card.PropertyWildRed;
                                        break;
                                    case 7:
                                        MainWindow.propertyCard = Card.PropertyWildYellow;
                                        break;
                                    case 8:
                                        MainWindow.propertyCard = Card.PropertyWildGreen;
                                        break;
                                    case 9:
                                        MainWindow.propertyCard = Card.PropertyWildBlack;
                                        break;
                                }
                            }
                            playCardAsProperty();
                            break;
                    }
                    return;
                #endregion

                #region moveCardsDecideType
                case turnStage.moveCardsDecideType:
                    switch (MainWindow.propertyCard)
                    {
                        case Card.PropertyBlackGreen__4:
                            MainWindow.propIndex2 = 8;
                            checkMoveCard();
                            return;
                        case Card.PropertyBlackLightBlue__4:
                            MainWindow.propIndex2 = 3;
                            checkMoveCard();
                            return;
                        case Card.PropertyBlackUtility__2:
                            MainWindow.propIndex2 = 1;
                            checkMoveCard();
                            return;
                        case Card.PropertyBlueGreen__4:
                            MainWindow.propIndex2 = 8;
                            checkMoveCard();
                            return;
                        case Card.PropertyBrownLightBlue__1:
                            MainWindow.propIndex2 = 3;
                            checkMoveCard();
                            return;
                        case Card.PropertyGreenBlack__4:
                            MainWindow.propIndex2 = 9;
                            checkMoveCard();
                            return;
                        case Card.PropertyGreenBlue__4:
                            MainWindow.propIndex2 = 2;
                            checkMoveCard();
                            return;
                        case Card.PropertyLightBlueBlack__4:
                            MainWindow.propIndex2 = 9;
                            checkMoveCard();
                            return;
                        case Card.PropertyLightBlueBrown__1:
                            MainWindow.propIndex2 = 0;
                            checkMoveCard();
                            return;
                        case Card.PropertyUtilityBlack__2:
                            MainWindow.propIndex2 = 9;
                            checkMoveCard();
                            return;
                        case Card.PropertyOrangePink__2:
                            MainWindow.propIndex2 = 4;
                            checkMoveCard();
                            return;
                        case Card.PropertyPinkOrange__2:
                            MainWindow.propIndex2 = 5;
                            checkMoveCard();
                            return;
                        case Card.PropertyRedYellow__3:
                            MainWindow.propIndex2 = 7;
                            checkMoveCard();
                            return;
                        case Card.PropertyYellowRed__3:
                            MainWindow.propIndex2 = 6;
                            checkMoveCard();
                            return;
                    }
                    return;
                #endregion

                case turnStage.moveCardsDecideTypeWild:
                    MainWindow.propIndex2 = 10;
                    checkMoveCard();
                    return;

                #region checkMoveCard
                case turnStage.checkMoveCard:
                    if (MainWindow.propertyType == PropertyType.Duo)
                    {
                        switch (MainWindow.propertyCard)
                        {
                            case Card.PropertyBlackGreen__4:
                                MainWindow.propertyCard = Card.PropertyGreenBlack__4;
                                break;
                            case Card.PropertyBlackLightBlue__4:
                                MainWindow.propertyCard = Card.PropertyLightBlueBlack__4;
                                break;
                            case Card.PropertyBlackUtility__2:
                                MainWindow.propertyCard = Card.PropertyUtilityBlack__2;
                                break;
                            case Card.PropertyBlueGreen__4:
                                MainWindow.propertyCard = Card.PropertyGreenBlue__4;
                                break;
                            case Card.PropertyBrownLightBlue__1:
                                MainWindow.propertyCard = Card.PropertyLightBlueBrown__1;
                                break;
                            case Card.PropertyGreenBlack__4:
                                MainWindow.propertyCard = Card.PropertyBlackGreen__4;
                                break;
                            case Card.PropertyGreenBlue__4:
                                MainWindow.propertyCard = Card.PropertyBlueGreen__4;
                                break;
                            case Card.PropertyLightBlueBlack__4:
                                MainWindow.propertyCard = Card.PropertyBlackLightBlue__4;
                                break;
                            case Card.PropertyLightBlueBrown__1:
                                MainWindow.propertyCard = Card.PropertyBrownLightBlue__1;
                                break;
                            case Card.PropertyUtilityBlack__2:
                                MainWindow.propertyCard = Card.PropertyBlackUtility__2;
                                break;
                            case Card.PropertyOrangePink__2:
                                MainWindow.propertyCard = Card.PropertyPinkOrange__2;
                                break;
                            case Card.PropertyPinkOrange__2:
                                MainWindow.propertyCard = Card.PropertyOrangePink__2;
                                break;
                            case Card.PropertyRedYellow__3:
                                MainWindow.propertyCard = Card.PropertyYellowRed__3;
                                break;
                            case Card.PropertyYellowRed__3:
                                MainWindow.propertyCard = Card.PropertyRedYellow__3;
                                break;
                        }
                    }
                    if (MainWindow.propertyType == PropertyType.Wild)
                    {
                        switch (MainWindow.propIndex2)
                        {
                            case 0:
                                MainWindow.propertyCard = Card.PropertyWildBrown;
                                break;
                            case 1:
                                MainWindow.propertyCard = Card.PropertyWildUtility;
                                break;
                            case 2:
                                MainWindow.propertyCard = Card.PropertyWildBlue;
                                break;
                            case 3:
                                MainWindow.propertyCard = Card.PropertyWildLightBlue;
                                break;
                            case 4:
                                MainWindow.propertyCard = Card.PropertyWildPink;
                                break;
                            case 5:
                                MainWindow.propertyCard = Card.PropertyWildOrange;
                                break;
                            case 6:
                                MainWindow.propertyCard = Card.PropertyWildRed;
                                break;
                            case 7:
                                MainWindow.propertyCard = Card.PropertyWildYellow;
                                break;
                            case 8:
                                MainWindow.propertyCard = Card.PropertyWildGreen;
                                break;
                            case 9:
                                MainWindow.propertyCard = Card.PropertyWildBlack;
                                break;
                            case 10:
                                MainWindow.propertyCard = Card.PropertyWild;
                                break;
                        }
                    }
                    finishMoveCard();
                    return;
                #endregion

                case turnStage.discard:
                    return;

                case turnStage.checkDiscard:
                    discardCard();
                    return;

                case turnStage.house:
                    MainWindow.propIndex = MainWindow.monopolyIndex1;
                    checkHouse();
                    return;

                case turnStage.checkHouse:
                    house();
                    return;

                case turnStage.hotel:
                    MainWindow.propIndex = MainWindow.monopolyIndex1;
                    checkHotel();
                    return;

                case turnStage.checkHotel:
                    hotel();
                    return;

                case turnStage.debtCollect:
                    debtCollect();
                    return;

                case turnStage.rentWild2:
                    rentWild();
                    return;

                #region doubleRentWild
                case turnStage.doubleRentWild:
                    MainWindow.payment = 2 * MainWindow.payment;
                    if (MainWindow.doublePlayed)
                    {
                        MainWindow.doublePlayed2 = true;
                    }
                    else
                    {
                        MainWindow.doublePlayed = true;
                    }
                    if ((MainWindow.playNum == 0) && (MainWindow.AllHands[MainWindow.playerNum].IndexOf(Card.DoubleTheRent__1) != MainWindow.AllHands[MainWindow.playerNum].LastIndexOf(Card.DoubleTheRent__1)))
                    {
                        askDoubleRentWild();
                        return;
                    }
                    playRentWild2();
                    return;
                #endregion

                #region doubleRent
                case turnStage.doubleRent:
                    MainWindow.payment = 2 * MainWindow.payment;
                    if (MainWindow.doublePlayed)
                    {
                        MainWindow.doublePlayed2 = true;
                    }
                    else
                    {
                        MainWindow.doublePlayed = true;
                    }
                    if ((MainWindow.playNum == 0) && (MainWindow.AllHands[MainWindow.playerNum].IndexOf(Card.DoubleTheRent__1) != MainWindow.AllHands[MainWindow.playerNum].LastIndexOf(Card.DoubleTheRent__1)))
                    {
                        askDoubleRent();
                        return;
                    }
                    checkRent();
                    return;
                #endregion

                #region rent
                case turnStage.rent:
                    switch (MainWindow.rentCard)
                    {
                        case Card.RentBlackUtility__1:
                            MainWindow.propIndex = 9;
                            break;

                        case Card.RentBrownLightBlue__1:
                            MainWindow.propIndex = 0;
                            break;

                        case Card.RentGreenBlue__1:
                            MainWindow.propIndex = 8;
                            break;

                        case Card.RentPinkOrange__1:
                            MainWindow.propIndex = 4;
                            break;

                        case Card.RentRedYellow__1:
                            MainWindow.propIndex = 6;
                            break;
                    }
                    MainWindow.payment = MainWindow.getRentAmount();
                    if ((MainWindow.AllHands[MainWindow.playerNum].Contains(Card.DoubleTheRent__1)) && (MainWindow.playNum < 2))
                    {
                        askDoubleRent();
                    }
                    else
                    {
                        MainWindow.playerTurns[MainWindow.playerNum].checkRent();
                    }
                    return;
                #endregion

                case turnStage.checkRent:
                    rent();
                    return;

                case turnStage.birthday:
                    collectBirthday();
                    return;

                case turnStage.slyDeal:
                    slyDeal();
                    return;

                case turnStage.forcedDeal2:
                    forcedDeal();
                    return;

                #region receiveForcedDeal
                case turnStage.receiveForcedDeal:
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    buttonBack.Visibility = System.Windows.Visibility.Hidden;
                    switch (MainWindow.propertyCard)
                    {
                        case Card.PropertyBlackGreen__4:
                            MainWindow.propertyCard = Card.PropertyBlackGreen__4;
                            break;
                        case Card.PropertyBlackLightBlue__4:
                            MainWindow.propertyCard = Card.PropertyBlackLightBlue__4;
                            break;
                        case Card.PropertyBlackUtility__2:
                            MainWindow.propertyCard = Card.PropertyBlackUtility__2;
                            break;
                        case Card.PropertyBlueGreen__4:
                            MainWindow.propertyCard = Card.PropertyBlueGreen__4;
                            break;
                        case Card.PropertyBrownLightBlue__1:
                            MainWindow.propertyCard = Card.PropertyBrownLightBlue__1;
                            break;
                        case Card.PropertyGreenBlack__4:
                            MainWindow.propertyCard = Card.PropertyBlackGreen__4;
                            break;
                        case Card.PropertyGreenBlue__4:
                            MainWindow.propertyCard = Card.PropertyBlueGreen__4;
                            break;
                        case Card.PropertyLightBlueBlack__4:
                            MainWindow.propertyCard = Card.PropertyBlackLightBlue__4;
                            break;
                        case Card.PropertyLightBlueBrown__1:
                            MainWindow.propertyCard = Card.PropertyBrownLightBlue__1;
                            break;
                        case Card.PropertyUtilityBlack__2:
                            MainWindow.propertyCard = Card.PropertyBlackUtility__2;
                            break;
                        case Card.PropertyOrangePink__2:
                            MainWindow.propertyCard = Card.PropertyOrangePink__2;
                            break;
                        case Card.PropertyPinkOrange__2:
                            MainWindow.propertyCard = Card.PropertyOrangePink__2;
                            break;
                        case Card.PropertyRedYellow__3:
                            MainWindow.propertyCard = Card.PropertyRedYellow__3;
                            break;
                        case Card.PropertyYellowRed__3:
                            MainWindow.propertyCard = Card.PropertyRedYellow__3;
                            break;
                    }
                    MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
                    if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex])
                    {
                        MainWindow.AllTableProperties[MainWindow.chosenPlayer][10].Add(MainWindow.propertyCard);
                        MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                        return;
                    }
                    else
                    {
                        MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Add(MainWindow.propertyCard);
                        MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                        return;
                    }
                #endregion

                case turnStage.dealBreaker:
                    MainWindow.propIndex = MainWindow.monopolyIndex1;
                    dealBreak();
                    return;

                #region acknowledgeAttack
                case turnStage.acknowledgeAttack1:
                    string stringPlayer = buttonPlayer.Content.ToString();
                    int player = (stringPlayer[7] - 49);
                    if (player == MainWindow.playerNum)
                    {
                        finishPlayAction();
                        return;
                    }
                    MainWindow.stage = turnStage.acknowledgeAttack2;
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    switch (MainWindow.playedCard)
                    {
                        case Card.Birthday__2:
                            pay();
                            break;

                        case Card.DealBreaker__5:
                            foreach (Card card in MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex])
                            {
                                MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(card);
                                //MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Remove(card);
                            }
                            MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Clear();
                            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.DealBreaker__5);
                            MainWindow.playedCard = Card.DealBreaker__5;
                            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";
                            MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                            break;

                        case Card.ForcedDeal__3:
                            MainWindow.propertyCard = MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum];
                            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex2].Add(MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2][MainWindow.cardNum2]);
                            //MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Add(MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex][MainWindow.cardNum]);
                            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].RemoveAt(MainWindow.cardNum);
                            MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2].RemoveAt(MainWindow.cardNum2);
                            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.ForcedDeal__3);
                            MainWindow.playedCard = Card.ForcedDeal__3;
                            //PropertyType propType = MainWindow.getPropertyType(MainWindow.propertyCard);
                            MainWindow.playerTurns[MainWindow.chosenPlayer].receiveForcedDeal();
                            break;

                        case Card.SlyDeal__3:
                            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex][MainWindow.cardNum]);
                            MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].RemoveAt(MainWindow.cardNum);
                            MainWindow.AllHands[MainWindow.playerNum].Remove(Card.SlyDeal__3);
                            MainWindow.playedCard = Card.SlyDeal__3;
                            Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";
                            MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                            break;

                        case Card.DebtCollector__3:
                            pay();
                            break;

                        case Card.RentWild__3:
                            pay();
                            break;

                        case Card.Rent:
                            pay();
                            break;
                    }
                    return;
                #endregion
            }
            //MainWindow.sendGameStates();
        }

        public void button2_Click(object sender = null, RoutedEventArgs e = null)
        {
            switch (MainWindow.stage)
            {
                case turnStage.begin:
                    return;

                case turnStage.decidePlayType:
                    moveCards();
                    return;

                case turnStage.playCardFromeHand:
                    return;

                case turnStage.decideCardType:
                    MainWindow.cardType = CardType.Money;
                    checkPlayCard();
                    return;

                #region decidePropertyType
                case turnStage.decidePropertyType:
                    switch (MainWindow.propertyCard)
                    {
                        case Card.PropertyBlackGreen__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlack__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlackLightBlue__4:
                            MainWindow.propertyCard = Card.PropertyLightBlueBlack__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlackUtility__2:
                            MainWindow.propertyCard = Card.PropertyUtilityBlack__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyBlueGreen__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlue__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyBrownLightBlue__1:
                            MainWindow.propertyCard = Card.PropertyLightBlueBrown__1;
                            checkPlayCard();
                            return;
                        case Card.PropertyGreenBlack__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlack__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyGreenBlue__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlue__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyLightBlueBlack__4:
                            MainWindow.propertyCard = Card.PropertyLightBlueBlack__4;
                            checkPlayCard();
                            return;
                        case Card.PropertyLightBlueBrown__1:
                            MainWindow.propertyCard = Card.PropertyLightBlueBrown__1;
                            checkPlayCard();
                            return;
                        case Card.PropertyUtilityBlack__2:
                            MainWindow.propertyCard = Card.PropertyUtilityBlack__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyOrangePink__2:
                            MainWindow.propertyCard = Card.PropertyPinkOrange__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyPinkOrange__2:
                            MainWindow.propertyCard = Card.PropertyPinkOrange__2;
                            checkPlayCard();
                            return;
                        case Card.PropertyRedYellow__3:
                            MainWindow.propertyCard = Card.PropertyYellowRed__3;
                            checkPlayCard();
                            return;
                        case Card.PropertyYellowRed__3:
                            MainWindow.propertyCard = Card.PropertyYellowRed__3;
                            checkPlayCard();
                            return;
                    }
                    return;
                #endregion

                case turnStage.checkPlayCard:
                    decidePlayType();
                    return;

                case turnStage.checkMoveCard:
                    decidePlayType();
                    return;

                case turnStage.discard:
                    return;

                case turnStage.checkDiscard:
                    return;

                case turnStage.house:
                    MainWindow.propIndex = MainWindow.monopolyIndex2;
                    checkHouse();
                    return;

                case turnStage.checkHouse:
                    decidePlayType();
                    return;

                case turnStage.hotel:
                    MainWindow.propIndex = MainWindow.monopolyIndex2;
                    checkHotel();
                    return;

                case turnStage.checkHotel:
                    decidePlayType();
                    return;

                case turnStage.debtCollect:
                    decidePlayType();
                    return;

                case turnStage.rentWild2:
                    decidePlayType();
                    return;

                case turnStage.doubleRentWild:
                    playRentWild2();
                    return;

                case turnStage.doubleRent:
                    checkRent();
                    return;

                #region rent
                case turnStage.rent:
                    switch (MainWindow.rentCard)
                    {
                        case Card.RentBlackUtility__1:
                            MainWindow.propIndex = 1;
                            break;

                        case Card.RentBrownLightBlue__1:
                            MainWindow.propIndex = 3;
                            break;

                        case Card.RentGreenBlue__1:
                            MainWindow.propIndex = 2;
                            break;

                        case Card.RentPinkOrange__1:
                            MainWindow.propIndex = 5;
                            break;

                        case Card.RentRedYellow__1:
                            MainWindow.propIndex = 7;
                            break;
                    }
                    MainWindow.payment = MainWindow.getRentAmount();
                    if ((MainWindow.AllHands[MainWindow.playerNum].Contains(Card.DoubleTheRent__1)) && (MainWindow.playNum < 2))
                    {
                        askDoubleRent();
                    }
                    else
                    {
                        MainWindow.playerTurns[MainWindow.playerNum].checkRent();
                    }
                    return;
                #endregion

                case turnStage.checkRent:
                    decidePlayType();
                    return;

                case turnStage.birthday:
                    decidePlayType();
                    return;

                case turnStage.slyDeal:
                    decidePlayType();
                    return;

                case turnStage.forcedDeal2:
                    decidePlayType();
                    return;

                #region receiveForcedDeal
                case turnStage.receiveForcedDeal:
                    button1.Visibility = System.Windows.Visibility.Hidden;
                    button2.Visibility = System.Windows.Visibility.Hidden;
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    buttonBack.Visibility = System.Windows.Visibility.Hidden;
                    switch (MainWindow.propertyCard)
                    {
                        case Card.PropertyBlackGreen__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlack__4;
                            break;
                        case Card.PropertyBlackLightBlue__4:
                            MainWindow.propertyCard = Card.PropertyLightBlueBlack__4;
                            break;
                        case Card.PropertyBlackUtility__2:
                            MainWindow.propertyCard = Card.PropertyUtilityBlack__2;
                            break;
                        case Card.PropertyBlueGreen__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlue__4;
                            break;
                        case Card.PropertyBrownLightBlue__1:
                            MainWindow.propertyCard = Card.PropertyLightBlueBrown__1;
                            break;
                        case Card.PropertyGreenBlack__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlack__4;
                            break;
                        case Card.PropertyGreenBlue__4:
                            MainWindow.propertyCard = Card.PropertyGreenBlue__4;
                            break;
                        case Card.PropertyLightBlueBlack__4:
                            MainWindow.propertyCard = Card.PropertyLightBlueBlack__4;
                            break;
                        case Card.PropertyLightBlueBrown__1:
                            MainWindow.propertyCard = Card.PropertyLightBlueBrown__1;
                            break;
                        case Card.PropertyUtilityBlack__2:
                            MainWindow.propertyCard = Card.PropertyUtilityBlack__2;
                            break;
                        case Card.PropertyOrangePink__2:
                            MainWindow.propertyCard = Card.PropertyPinkOrange__2;
                            break;
                        case Card.PropertyPinkOrange__2:
                            MainWindow.propertyCard = Card.PropertyPinkOrange__2;
                            break;
                        case Card.PropertyRedYellow__3:
                            MainWindow.propertyCard = Card.PropertyYellowRed__3;
                            break;
                        case Card.PropertyYellowRed__3:
                            MainWindow.propertyCard = Card.PropertyYellowRed__3;
                            break;
                    }
                    MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
                    if (MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex])
                    {
                        MainWindow.AllTableProperties[MainWindow.chosenPlayer][10].Add(MainWindow.propertyCard);
                        MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                        return;
                    }
                    else
                    {
                        MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Add(MainWindow.propertyCard);
                        MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                        return;
                    }
                #endregion

                case turnStage.dealBreaker:
                    MainWindow.propIndex = MainWindow.monopolyIndex2;
                    dealBreak();
                    return;

                #region acknowledgeAttack
                case turnStage.acknowledgeAttack1:
                    string stringPlayer = buttonPlayer.Content.ToString();
                    int player = (stringPlayer[7] - 49);
                    MainWindow.justSayNos++;
                    if (player == MainWindow.playerNum)
                    {
                        justSayNoOwner();
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "Just Said No to your Just Say No.  Get rocked...";
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].button1.Content = "OK";
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].button1.Visibility = System.Windows.Visibility.Visible;
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Visibility = System.Windows.Visibility.Hidden;
                        //if (MainWindow.AllHands[MainWindow.chosenPlayer].Contains(Card.JustSayNo__4))
                        //{
                        //    MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Content = "Just Say No";
                        //    MainWindow.playerTurns[MainWindow.chosenPlayer].button2.Visibility = System.Windows.Visibility.Visible;
                        //}
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].button3.Visibility = System.Windows.Visibility.Hidden;
                        //MainWindow.playerTurns[MainWindow.chosenPlayer].buttonBack.Visibility = System.Windows.Visibility.Hidden;
                        //button1.Visibility = System.Windows.Visibility.Hidden;
                        //button2.Visibility = System.Windows.Visibility.Hidden;
                        //button3.Visibility = System.Windows.Visibility.Hidden;
                        //buttonBack.Visibility = System.Windows.Visibility.Hidden;
                        //switch (MainWindow.playedCard)
                        //{
                        //    case Card.Birthday__2:
                        //        MainWindow.payersLeft.Add(MainWindow.chosenPlayer);
                        //        foreach (int i in MainWindow.payersLeft)
                        //        {
                        //            Prompt.Content += MainWindow.playerNames[i] + ", ";
                        //        }
                        //        break;

                        //    case Card.Rent:
                        //        foreach (int i in MainWindow.payersLeft)
                        //        {
                        //            Prompt.Content += MainWindow.playerNames[i] + ", ";
                        //        }
                        //        break;
                        //}
                        //return;
                    }
                    else //Other player just says no...
                    {
                        if (MainWindow.justSayNos == 1)
                        {
                            justSayNo();
                        }
                        else
                        {
                            justSayNoAgain();
                        }
                    }
                    return;
                #endregion
            }
            //MainWindow.sendGameStates();
        }

        public void button3_Click(object sender = null, RoutedEventArgs e = null)
        {
            switch (MainWindow.stage)
            {
                case turnStage.begin:
                    return;

                case turnStage.decidePlayType:
                    endTurn();
                    return;

                case turnStage.playCardFromeHand:
                    return;

                case turnStage.decideCardType:
                    MainWindow.cardType = CardType.Property;
                    MainWindow.propertyCard = MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum];
                    Card card = MainWindow.AllHands[MainWindow.playerNum][MainWindow.cardNum];
                    if ((card == Card.PropertyBlackGreen__4) || (card == Card.PropertyBlackLightBlue__4) || (card == Card.PropertyBlackUtility__2) || (card == Card.PropertyBlueGreen__4) || (card == Card.PropertyBrownLightBlue__1) || (card == Card.PropertyGreenBlack__4) || (card == Card.PropertyGreenBlue__4) || (card == Card.PropertyLightBlueBlack__4) || (card == Card.PropertyLightBlueBrown__1) || (card == Card.PropertyUtilityBlack__2) || (card == Card.PropertyOrangePink__2) || (card == Card.PropertyPinkOrange__2) || (card == Card.PropertyRedYellow__3) || (card == Card.PropertyYellowRed__3))
                    {
                        decidePropertyType();
                    }
                    else if (card == Card.PropertyWild)
                    {
                        decidePropertyTypeWild();
                    }
                    else
                    {
                        checkPlayCard();
                    }
                    return;

                case turnStage.checkPlayCard:
                    return;

                case turnStage.discard:
                    return;

                case turnStage.receiveForcedDeal:
                    MainWindow.AllTableProperties[MainWindow.chosenPlayer][10].Add(Card.PropertyWild);
                    MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].RemoveAt(MainWindow.cardNum);
                    finishPlayAction();
                    return;

                case turnStage.acknowledgeAttack2:
                    string toDisplay = MainWindow.playerNames[MainWindow.chosenPlayer] + " paid the following cards to " + MainWindow.playerNames[MainWindow.playerNum] + ": ";
                    foreach (Card item in MainWindow.tableMoneySelectedItems)
                    {
                        toDisplay += item.ToString() + ", ";
                        MainWindow.AllTableMoney[MainWindow.playerNum].Add(item);
                        MainWindow.AllTableMoney[MainWindow.chosenPlayer].Remove(item);
                    }
                    foreach (Card item in MainWindow.tablePropertiesSelectedItems)
                    {
                        toDisplay += item.ToString() + ", ";
                        MainWindow.propIndex = MainWindow.getPropertyIndex(item);
                        if (MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Count >= MainWindow.monopolyPropsNeeded[MainWindow.propIndex])
                        {
                            MainWindow.AllTableProperties[MainWindow.playerNum][10].Add(item);
                        }
                        else
                        {
                            MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].Add(item);
                        }
                        MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].Remove(item);
                    }
                    showTable();
                    updateUniversalPrompt(toDisplay);
                    MainWindow.payersLeft.Remove(MainWindow.chosenPlayer);
                    button3.Visibility = System.Windows.Visibility.Hidden;
                    //Table_Money.IsEnabled = false;
                    //Table_Properties.IsEnabled = false;
                    Prompt.Content = MainWindow.playerNames[MainWindow.playerNum] + "'s turn...";
                    MainWindow.playerTurns[MainWindow.playerNum].finishPlayAction();
                    return;
            }
            //MainWindow.sendGameStates();
        }

        public void buttonBack_Click(object sender = null, RoutedEventArgs e = null)
        {
            switch (MainWindow.stage)
            {
                case turnStage.begin:
                    return;

                case turnStage.decidePlayType:
                    return;

                case turnStage.playCardFromeHand:
                    decidePlayType();
                    return;

                case turnStage.decideCardType:
                    playCardFromHand();
                    return;

                case turnStage.decidePropertyType:
                    decidePlayType();
                    return;

                case turnStage.decidePropertyTypeWild:
                    decidePlayType();
                    return;

                case turnStage.moveCards:
                    decidePlayType();
                    return;

                case turnStage.moveCardsDecideType:
                    decidePlayType();
                    return;

                case turnStage.moveCardsDecideTypeWild:
                    decidePlayType();
                    return;

                case turnStage.checkPlayCard:
                    decideCardType();
                    return;

                case turnStage.discard:
                    decidePlayType();
                    return;

                case turnStage.checkDiscard:
                    decidePlayType();
                    return;

                case turnStage.house:
                    decidePlayType();
                    return;

                case turnStage.checkHotel:
                    decidePlayType();
                    return;

                case turnStage.hotel:
                    decidePlayType();
                    return;

                case turnStage.checkHouse:
                    decidePlayType();
                    return;

                case turnStage.debtCollect:
                    decidePlayType();
                    return;

                case turnStage.rentWild:
                    decidePlayType();
                    return;

                case turnStage.rentWild2:
                    playRentWild();
                    return;

                case turnStage.doubleRentWild:
                    decidePlayType();
                    return;

                case turnStage.doubleRent:
                    decidePlayType();
                    return;

                case turnStage.checkRent:
                    decidePlayType();
                    return;

                case turnStage.rent:
                    decidePlayType();
                    return;

                case turnStage.birthday:
                    decidePlayType();
                    return;

                case turnStage.slyDeal:
                    decidePlayType();
                    return;

                case turnStage.forcedDeal1:
                    decidePlayType();
                    return;

                case turnStage.forcedDeal2:
                    playForcedDeal();
                    return;

                case turnStage.dealBreaker:
                    decidePlayType();
                    return;
            }
            //MainWindow.sendGameStates();
        }

        //public void Hand_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (Hand.SelectedIndex < 0)
        //    {
        //        return;
        //    }
        //    if (MainWindow.stage == turnStage.playCardFromeHand)
        //    {
        //        MainWindow.cardNum = Hand.SelectedIndex;
        //        decideCardType();
        //    }

        //    if (MainWindow.stage == turnStage.discard)
        //    {
        //        MainWindow.cardNum = Hand.SelectedIndex;
        //        checkDiscard();
        //    }

        //    if (MainWindow.stage == turnStage.slyDeal)
        //    {
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkSlyDeal();
        //        }
        //        MainWindow.cardNum = Hand.SelectedIndex;
        //    }
        //}

        //public void buttonPlayer_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MainWindow.stage == turnStage.debtCollect)
        //    {
        //        string chosenPlayer = buttonPlayer.Content.ToString();
        //        MainWindow.chosenPlayer = (chosenPlayer[7] - 49);
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkDebtCollector();
        //        }
        //    }
        //    if (MainWindow.stage == turnStage.rentWild2)
        //    {
        //        string chosenPlayer = buttonPlayer.Content.ToString();
        //        MainWindow.chosenPlayer = (chosenPlayer[7] - 49);
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].checkRentWild();
        //        }
        //    }
        //    if (MainWindow.stage == turnStage.dealBreaker)
        //    {
        //        string chosenPlayer = buttonPlayer.Content.ToString();
        //        MainWindow.chosenPlayer = (chosenPlayer[7] - 49);
        //        if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].playDealBreaker2();
        //        }
        //    }
        //}

        //public void Table_Properties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (MainWindow.stage == turnStage.acknowledgeAttack)
        //    {
        //        int totalValue = 0;
        //        int numOfWilds = 0;
        //        foreach (Card card in Table_Properties.Items)
        //        {
        //            if (MainWindow.getPropertyType(card) == PropertyType.Wild)
        //            {
        //                numOfWilds++;
        //            }
        //        }
        //        foreach (Card card in Table_Properties.SelectedItems)
        //        {
        //            if (MainWindow.getPropertyType(card) == PropertyType.Wild)
        //            {
        //                numOfWilds--;
        //            }
        //        }
        //        foreach (Card card in Table_Properties.SelectedItems)
        //        {
        //            totalValue += MainWindow.getValue(card);

        //        }
        //        foreach (Card card in Table_Money.SelectedItems)
        //        {
        //            totalValue += MainWindow.getValue(card);
        //        }
        //        if ((totalValue >= MainWindow.payment) || ((Table_Properties.SelectedItems.Count >= (Table_Properties.Items.Count-numOfWilds)) && (Table_Money.SelectedItems.Count == Table_Money.Items.Count)))
        //        {
        //            button3.Content = "Make Payment";
        //            button3.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            button3.Visibility = System.Windows.Visibility.Hidden;
        //        }
        //    }
        //}

        //public void Table_Money_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (MainWindow.stage == turnStage.acknowledgeAttack)
        //    {
        //        int totalValue = 0;
        //        int numOfWilds = 0;
        //        foreach (Card card in Table_Properties.Items)
        //        {
        //            if (MainWindow.getPropertyType(card) == PropertyType.Wild)
        //            {
        //                numOfWilds++;
        //            }
        //        }
        //        foreach (Card card in Table_Properties.SelectedItems)
        //        {
        //            if (MainWindow.getPropertyType(card) == PropertyType.Wild)
        //            {
        //                numOfWilds--;
        //            }
        //        }
        //        foreach (Card card in Table_Properties.SelectedItems)
        //        {
        //            totalValue += MainWindow.getValue(card);
        //        }
        //        foreach (Card card in Table_Money.SelectedItems)
        //        {
        //            totalValue += MainWindow.getValue(card);
        //        }
        //        if ((totalValue >= MainWindow.payment) || (((Table_Properties.SelectedItems.Count == Table_Properties.Items.Count-numOfWilds)) && (Table_Money.SelectedItems.Count == Table_Money.Items.Count)))
        //        {
        //            button3.Content = "Make Payment";
        //            button3.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            button3.Visibility = System.Windows.Visibility.Hidden;
        //        }
        //    }
        //}

        //public void Table_Properties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (Table_Properties.SelectedIndex < 0)
        //    {
        //        return;
        //    }
        //    //if (MainWindow.stage == turnStage.slyDeal)
        //    //{
        //    //    MainWindow.cardNum = Table_Properties.SelectedIndex;
        //    //    MainWindow.propertyCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //    //    string chosenPlayer = buttonPlayer.Content.ToString();
        //    //    MainWindow.chosenPlayer = (chosenPlayer[7] - 49);
        //    //    MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //    //    MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex].IndexOf(MainWindow.propertyCard);
        //    //    if (MainWindow.chosenPlayer != MainWindow.playerNum)
        //    //    {
        //    //        MainWindow.playerTurns[MainWindow.playerNum].checkSlyDeal();
        //    //    }
        //    //}

        //    if (MainWindow.stage == turnStage.forcedDeal1)
        //    {
        //        MainWindow.cardNum = Table_Properties.SelectedIndex;
        //        Card currentCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //        MainWindow.propIndex = MainWindow.getPropertyIndex(currentCard);
        //        MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].IndexOf(currentCard);
        //        MainWindow.playerTurns[MainWindow.playerNum].playForcedDeal2();
        //    }
        //    //else if (MainWindow.stage == turnStage.forcedDeal2)
        //    //{
        //    //    MainWindow.cardNum2 = Table_Properties.SelectedIndex;
        //    //    MainWindow.propertyCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum2);
        //    //    MainWindow.propIndex2 = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //    //    string chosenPlayer = buttonPlayer.Content.ToString();
        //    //    MainWindow.chosenPlayer = (chosenPlayer[7] - 49);
        //    //    MainWindow.cardNum2 = MainWindow.AllTableProperties[MainWindow.chosenPlayer][MainWindow.propIndex2].IndexOf(MainWindow.propertyCard);
        //    //    MainWindow.playerTurns[MainWindow.playerNum].checkForcedDeal();
        //    //}

        //    if (MainWindow.stage == turnStage.rentWild)
        //    {
        //        MainWindow.cardNum = Table_Properties.SelectedIndex;
        //        Card currentCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //        MainWindow.propIndex = MainWindow.getPropertyIndex(currentCard);
        //        MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].IndexOf(currentCard);
        //        MainWindow.payment = MainWindow.getRentAmount();
        //        if ((MainWindow.AllHands[MainWindow.playerNum].Contains(Card.DoubleTheRent__1)) && (MainWindow.playNum < 2))
        //        {
        //            askDoubleRentWild();
        //        }
        //        else
        //        {
        //            MainWindow.playerTurns[MainWindow.playerNum].playRentWild2();
        //        }
        //    }

        //    if (MainWindow.stage == turnStage.decidePropertyTypeWild)
        //    {
        //        MainWindow.cardNum2 = Table_Properties.SelectedIndex;
        //        MainWindow.propertyCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum2);
        //        MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //        checkPlayCard();
        //    }

        //    if (MainWindow.stage == turnStage.moveCards)
        //    {
        //        MainWindow.cardNum = Table_Properties.SelectedIndex;
        //        MainWindow.propertyCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum);
        //        //Problem... propIndex only helps IFF the property is in the normal stack.  This won't work for extra [10] stack
        //        //So... I will first check the extra stack [10] when I remove a card...
        //        if (MainWindow.AllTableProperties[MainWindow.playerNum][10].Contains(MainWindow.propertyCard))
        //        {
        //            MainWindow.propIndex = 10;
        //        }
        //        else
        //        {
        //            MainWindow.propIndex = MainWindow.getPropertyIndex(MainWindow.propertyCard);
        //        }
        //        MainWindow.cardNum = MainWindow.AllTableProperties[MainWindow.playerNum][MainWindow.propIndex].IndexOf(MainWindow.propertyCard);
        //        MainWindow.propertyType = MainWindow.getPropertyType(MainWindow.propertyCard);
        //        if (MainWindow.propertyType == PropertyType.Duo)
        //        {
        //            moveCardsDecideType();
        //        }
        //        else if (MainWindow.propertyType == PropertyType.Wild)
        //        {
        //            moveCardsDecideTypeWild();
        //        }
        //        else
        //        {
        //            Prompt.Content = "This card cannot be moved.";
        //            button1.Visibility = System.Windows.Visibility.Hidden;
        //            button2.Visibility = System.Windows.Visibility.Hidden;
        //            button3.Visibility = System.Windows.Visibility.Hidden;
        //            buttonBack.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        return;
        //    }
        //    if (MainWindow.stage == turnStage.moveCardsDecideTypeWild)
        //    {
        //        MainWindow.cardNum2 = Table_Properties.SelectedIndex;
        //        Card currentCard = Table_Properties.Items.Cast<Card>().ElementAt(MainWindow.cardNum2);
        //        MainWindow.propIndex2 = MainWindow.getPropertyIndex(currentCard);
        //        checkMoveCard();
        //    }
        //}
        #endregion
    }
}
