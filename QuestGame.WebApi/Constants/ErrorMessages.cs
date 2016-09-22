using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Constants
{
    public class ErrorMessages
    {
        public const string BadRequest = "Неудачный запрос!";

        public const string QuestNotFound = "Квест не найден!";
        public const string QuestNotCreate = "Не удалось добавить квест!";
        public const string QuestNotDelete = "Не удалось удалить квест!";
        public const string QuestNotUdate = "Не удалось обновить квест!";

        public const string StageNotFound = "Сцена не найдена!";
        public const string StageNotCreate = "Не удалось добавить сцену!";
        public const string StageNotUpdate = "Не удалось обновить сцену!";
        public const string StageNotDelete = "Не удалось удалить сцену!";

        public const string MotionNotFound = "Действие не найдено!";
        public const string MotionNotCreate = "Не удалось добавить действие!";
        public const string MotionNotUpdate = "Не удалось обновить действие!";
        public const string MotionNotDelete = "Не удалось удалить действие!";
    }
}