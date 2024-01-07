using System;

namespace LifeInDiary.Libs
{
    /// <summary>
    /// Time format for diary time
    /// </summary>
    public enum DiaryTimeFormat
    {
        /// <summary>
        /// 12 hour format
        /// </summary>
        _12 = 0,
        /// <summary>
        /// 24 hour format
        /// </summary>
        _24 = 1
    }
    public enum DiaryDateFormat
    {
        US = 0,
        British = 1
    }

    public static class DiaryComponents
    {
        /// <summary>
        /// Get current day of the week
        /// </summary>
        /// <returns></returns>
        public static string GetDay()
        {
            string Day = string.Empty;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            Day = currentDay.ToString();

            return Day;
        }

        public static string GetDate(DiaryDateFormat dateFormat)
        {
            string Date = string.Empty;
            if (dateFormat == DiaryDateFormat.British)
            {
                Date = $"{GetFormatedDayNumber(DateTime.Now.Date.Day)} {GetMonthName(DateTime.Now.Month)}, {DateTime.Now.Year}";
            }
            else
            {
                Date = $"{GetMonthName(DateTime.Now.Month)} {GetFormatedDayNumber(DateTime.Now.Date.Day)}, {DateTime.Now.Year}";

            }
            return Date;
        }
        /// <summary>
        /// Returns current time specific to the dateTimeFormat
        /// </summary>
        /// <param name="dateTimeFormat">The format of the time</param>
        /// <returns></returns>
        public static string GetTime(DiaryTimeFormat dateTimeFormat)
        {
            string time = string.Empty;
            if (dateTimeFormat == DiaryTimeFormat._24)
            {
                time = $"{DateTime.Now.Hour} : {DateTime.Now.Minute}";
            }
            else if (dateTimeFormat == DiaryTimeFormat._12)
            {
                int currentHour = DateTime.Now.Hour;
                string currentMinute = DateTime.Now.Minute.ToString();
                string Meridian = string.Empty;
                if (currentHour > 12)
                {
                    Meridian = "PM";
                    currentHour = currentHour - 12;
                }
                else if (currentHour < 12)
                {
                    Meridian = "AM";
                }
                if (int.Parse(currentMinute) < 10)
                {
                    currentMinute = $"0{currentMinute}";
                }
                time = $"{currentHour} : {currentMinute} {Meridian}";
            }
            return time;
        }

        public static string LoremString()
        {
            string text1 = "Lorem ipsum\tdolor sit amet consectetur adipisicing elit.\t  Dolorem velit assumenda pariatur a veritatis quisquam vitae sint\teveniet saepe\tnulla dignissimos doloremque corrupti reiciendis quae harum natus ipsa? Labore\tiste consequatur animi eos modi dolorum reprehenderit exercitationem\trecusandae excepturi rerum quis neque quae laborum fugiat fuga eum ut odit nesciunt\tImpedit nesciunt magni nulla libero ad eos praesentium voluptas quibusdam\t expedita\tdicta ut porro similique! Tenetur consectetur\tmagni praesentium\tveniam vero fugit veritatis aliquam est officia sint consequuntur laborum facilis ducimus dignissimos eum suscipit iusto\trecusandae debitis dolorem ratione temporibus quos nihil laudantium odio? Sunt minima deserunt animi totam quae quis reprehenderit sit eaque eum vero veritatis dicta neque a numquam laborum quam libero expedita\tpossimus velit\tConsequuntur\ttempora cupiditate architecto totam ipsum eius fugit autem sed quia accusantium dicta asperiores incidunt mollitia\tporro fuga necessitatibus dolorem commodi perferendis laudantium? Sint eaque optio quia excepturi tempora nisi exercitationem\tdolorem totam corporis\tperspiciatis porro odio\tQuisquam\talias impedit blanditiis earum eligendi vero quibusdam\tplaceat minus architecto incidunt\tillo veritatis adipisci atque? Ipsam non\tullam\tmolestiae quia\tprovident eum ab cum amet praesentium voluptatum sunt consequuntur corrupti ipsum\tVelit provident totam numquam? Libero nisi veritatis eum quas impedit maiores! Ipsa\tcum quisquam aliquam quidem in beatae nostrum quibusdam? Aliquid quod\trecusandae officiis quis vel atque blanditiis ducimus harum cumque nam neque tempora veniam velit illum tenetur deleniti exercitationem accusantium est quam porro nulla quos officia? \t\tVitae\ttemporibus\tQuas consectetur eos laudantium dolorum\tporro\tquod iste ut qui quo officia labore\tquis expedita ducimus! Architecto \tincidunt dolorem labore non\tsoluta accusamus minus\tDebitis\tautem error? Eligendi libero sed magni praesentium\tconsequatur debitis\tlabore itaque\tdicta maxime excepturi recusandae? Repellat modi nesciunt animi iusto consequatur rem corporis? Officia hic aliquam corrupti beatae deleniti quae saepe nesciunt \tfugit exercitationem harum\tblanditiis obcaecati eum commodi accusantium.";
            string text2 = "Lorem ipsum\tdolor sit amet consectetur adipisicing elit\t  Dolorem velit assumenda pariatur a veritatis quisquam vitae sint\teveniet saepe\tnulla dignissimos doloremque corrupti reiciendis quae harum natus ipsa? Labore\tiste consequatur animi eos modi dolorum reprehenderit exercitationem\trecusandae excepturi rerum quis neque quae laborum fugiat fuga eum ut odit nesciunt\tImpedit nesciunt magni nulla libero ad eos praesentium voluptas quibusdam\texpedita\tdicta ut porro similique! Tenetur consectetur\tmagni praesentium\tveniam vero fugit veritatis aliquam est officia sint consequuntur laborum facilis ducimus dignissimos eum suscipit iusto\trecusandae debitis dolorem ratione temporibus quos nihil laudantium odio? Sunt minima deserunt animi totam quae quis reprehenderit sit eaque eum vero veritatis dicta neque a numquam laborum quam libero expedita\tpossimus velit\t Consequuntur\ttempora cupiditate architecto totam ipsum eius fugit autem sed quia accusantium dicta asperiores incidunt mollitia\tporro fuga necessitatibus dolorem commodi perferendis laudantium? Sint eaque optio quia excepturi tempora nisi exercitationem\tdolorem totam corporis\tperspiciatis porro odio\t Quisquam\talias impedit blanditiis earum eligendi vero quibusdam\tplaceat minus architecto incidunt\tillo veritatis adipisci atque? Ipsam non\tullam\tmolestiae quia\tprovident eum ab cum amet praesentium voluptatum sunt consequuntur corrupti ipsum\t Velit provident totam numquam? Libero nisi veritatis eum quas impedit maiores! Ipsa\tcum quisquam aliquam quidem in beatae nostrum quibusdam? Aliquid quod\trecusandae officiis quis vel atque blanditiis ducimus harum cumque nam neque tempora veniam velit illum tenetur deleniti exercitationem accusantium est quam porro nulla quos officia? Vitae\t\t\t temporibus. Quas consectetur eos laudantium dolorum\tporro\tquod iste ut qui quo officia labore\tquis expedita ducimus! Architecto incidunt dolorem \t labore non\tsoluta accusamus minus. Debitis\tautem error? Eligendi libero sed magni praesentium\tconsequatur debitis\tlabore itaque\tdicta maxime excepturi recusandae? Repellat modi nesciunt animi iusto consequatur rem corporis? Officia hic aliquam corrupti beatae deleniti quae saepe nesciunt fugit exercitationem harum\t blanditiis obcaecati eum commodi accusantium.";
            string text3 = "Lorem ipsum\tdolor sit amet consectetur adipisicing elit\t  Dolorem velit assumenda pariatur a veritatis quisquam vitae sint\teveniet saepe\tnulla dignissimos doloremque corrupti reiciendis quae harum natus ipsa? Labore\tiste consequatur animi eos modi dolorum reprehenderit exercitationem\trecusandae excepturi rerum quis neque quae laborum fugiat fuga eum ut odit nesciunt\tImpedit nesciunt magni nulla libero ad eos praesentium voluptas quibusdam\texpedita\tdicta ut porro similique! Tenetur consectetur\tmagni praesentium\tveniam vero fugit veritatis aliquam est officia sint consequuntur laborum facilis ducimus dignissimos eum suscipit iusto\trecusandae debitis dolorem ratione temporibus quos nihil laudantium odio? Sunt minima deserunt animi totam quae quis reprehenderit sit eaque eum vero veritatis dicta neque a numquam laborum quam libero expedita\tpossimus velit\t Consequuntur\ttempora cupiditate architecto totam ipsum eius fugit autem sed quia accusantium dicta asperiores incidunt mollitia\tporro fuga necessitatibus dolorem commodi perferendis laudantium? Sint eaque optio quia excepturi tempora nisi exercitationem\tdolorem totam corporis\tperspiciatis porro odio\t Quisquam\talias impedit blanditiis earum eligendi vero quibusdam\tplaceat minus architecto incidunt\tillo veritatis adipisci atque? Ipsam non\tullam\tmolestiae quia\tprovident eum ab cum amet praesentium voluptatum sunt consequuntur corrupti ipsum\t Velit provident totam numquam? Libero nisi veritatis eum quas impedit maiores! Ipsa\tcum quisquam aliquam quidem in beatae nostrum quibusdam? Aliquid quod\trecusandae officiis quis vel atque blanditiis ducimus harum cumque nam neque tempora veniam velit illum tenetur deleniti exercitationem accusantium est quam porro nulla quos officia? Vitae\t\t\t temporibus. Quas consectetur eos laudantium dolorum\tporro\tquod iste ut qui quo officia labore\tquis expedita ducimus! Architecto incidunt dolorem \t labore non\tsoluta accusamus minus. Debitis\tautem error? Eligendi libero sed magni praesentium\tconsequatur debitis\tlabore itaque\tdicta maxime excepturi recusandae? Repellat modi nesciunt animi iusto consequatur rem corporis? Officia hic aliquam corrupti beatae deleniti quae saepe nesciunt fugit exercitationem harum\t blanditiis obcaecati eum commodi accusantium.";
            return text1 + text2 + text3;
        }


        private static string GetMonthName(int MonthNumber)
        {
            if (MonthNumber == 0) { return "error"; }

            string MonthName = string.Empty;
            string[] MonthArr = { null, "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            MonthName = MonthArr[MonthNumber];
            return MonthName;
        }
        private static string GetFormatedDayNumber(double day)
        {
            string formatedDay = "";

            string lastLetterOfDayNumber = day.ToString().Remove(0, day.ToString().Length - 1);
            if (lastLetterOfDayNumber == "1")
            {
                formatedDay = day + "st";
            }
            else if (lastLetterOfDayNumber == "2")
            {
                formatedDay = day + "nd";
            }
            else if (lastLetterOfDayNumber == "3")
            {
                formatedDay = day + "rd";
            }
            else
            {
                formatedDay = day + "th";
            }
            return formatedDay;
        }
    }
}
