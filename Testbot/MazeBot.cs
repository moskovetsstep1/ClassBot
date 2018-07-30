using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Testbot
{
    public class MazeBot
    {
        public readonly TelegramBotClient BotClient;


        public MazeBot(string _tMaze)
        {
            BotClient = new TelegramBotClient(_tMaze); //{"Timeout":"00:01:40","IsReceiving":true,"MessageOffset":0}
            BotClient.OnMessage += OnNewMessage;
            BotClient.StartReceiving();
            Console.ReadLine();
        }

        public void OnNewMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type != MessageType.Text)
                return;
            if (e.Message.Text == "/start")
            {
                BotClient.SendChatActionAsync(e.Message.Chat.Id, ChatAction.Typing);


                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new [] // first row
                    {
                   //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                        InlineKeyboardButton.WithCallbackData("⬆️ Вверх", "1"),
                   //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                    },
                    new [] // second row
                    {
                        InlineKeyboardButton.WithCallbackData("⬅️ Влево", "2"),
                   //     InlineKeyboardButton.WithCallbackData(" ", "1"),
                        InlineKeyboardButton.WithCallbackData("Вправо ➡️", "3"),
                    },
                    new [] // third row
                    {
                   //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                        InlineKeyboardButton.WithCallbackData("⬇️ Вниз", "4"),
                   //     InlineKeyboardButton.WithCallbackData(" ", "0"),
                    },
                });

                BotClient.SendTextMessageAsync(e.Message.Chat.Id,"Choose",replyMarkup: inlineKeyboard);
                BotClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            }
        }

        private void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data != "0")
            {
                switch (e.CallbackQuery.Data)
                {
                    case "1":
                        BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "вверх так вверх");
                        break;
                    case "2":
                        BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "влево");
                        break;
                    case "3":
                        BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "вправо");
                        break;
                    case "4":
                        BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "вниз так вниз");
                        break;
                }

                BotClient.OnCallbackQuery -= BotClient_OnCallbackQuery;
            }
            else
            {
                BotClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,
                    "Выбирайте направление а не пустые кнопки");
            }
        }


    }

}