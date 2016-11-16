using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Constants
{
    public class ErrorMessages
    {
        public const string BadRequest = "Неудачный запрос!";
        public const string InternalServerError = "Ошибка обработки данных!";

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

        public const string AccountSuccessRegister = "Пользователь успешно зарегистрирован!";
        public const string AccountFailRegister = "Ошибка регистрации!";
        public const string AccountFailLogin = "Неудачная попытка аутентификации!";
        public const string AccountUserNotFound = "Пользователь не найден!";
        public const string AccountConfirmEmailError = "Не удалось отправить код подтверждения, возможно сервер не доступен. Попробуйте еще раз позднее.<br/> Чтобы повторить письмо с подтверждением - перейдите в Ваш профиль пользователя.";
        public const string AccountUserExist = "Пользователь с таким email уже существует!";

        
        public const string BaseSuccessUploadFile = "Файл успешно отправлен!";
    }
}