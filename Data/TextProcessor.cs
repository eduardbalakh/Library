using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library.Data
{
    class TextProcessor
    {
        private static List<WordEntity> _wordlist = new List<WordEntity>();
        

        private static List<WordEntity> ReadFile<T>(string dir)//Reading all data and distinguishing repeated items
        {
            return File.ReadAllText(dir)
                .ToLower()
                .Split(new[] { ' ', ',', ';', '.', '—', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)//Separators are listed on the basis of testing
                .GroupBy(w => w)
                .Where(w => w.Key.Length > 2)//word length should be 3 minimum
                .Where(w => w.Count() > 3)//if we meet th word more than 3 times we treat it as frequently repeated word
                .Select(w => new WordEntity() { frequency = w.Count(), word = w.Key })//transform it to our WordEntity class
                //.OrderByDescending(w => w.frequency)
                //.ThenBy(w => w.word)
                .ToList();
        }

        public static RequestContext UpdateLibrary(string path)//Adding new file for extending our library
        {
            var context = new DictDbContext();
            try
            {
                if (context.Words.Any())
                {
                    if(!File.Exists(path)) return new RequestContext($"Файла не существует в указанной директории {path}", true);
                    var tempList = ReadFile<WordEntity>(path);
                    foreach (var newWord in tempList)
                    {
                        var word = context.Words.FirstOrDefault(w => w.word == newWord.word);
                        if (word != null) word.frequency += newWord.frequency;
                        else context.Words.Add(newWord);
                    }
                    context.SaveChanges();

                    _wordlist = context.Words.ToList();
                    return new RequestContext($"Словарь обновлен из файла {path}", false);
                }
                else
                {
                    return new RequestContext($"Словарь не обновлен, поскольку не был создан ранее. Для начала создайте словарь", true);
                }
            }
            catch (Exception e)
            {
                return new RequestContext("Exception in UpdateLibrary(): " + e.Message, true);
            }
            finally
            {
                context.Dispose();
            }

        }

        public static RequestContext CreateLibrary(string path)//creating library
        {
            var context = new DictDbContext();
            try
            {
                if (context.Words.Any()) return new RequestContext("Словарь уже существует. Для расширения существующего словаря воспользуйтесь командой обновить данные", true);
                
                if (!File.Exists(path)) return new RequestContext($"Файла не существует в указанной директории {path}", false);
                _wordlist = ReadFile<WordEntity>(path);
                context.AddRange(_wordlist);
                context.SaveChanges();
                _wordlist = context.Words.ToList();
            }
            catch (Exception e)
            {
                return new RequestContext("Exception in CreateLibrary(): " + e.Message, true);
            }
            finally{ context.Dispose();}

            return new RequestContext($"Словарь создан из файла {path}",false);
        }

        public static List<WordEntity> GetAllList => _wordlist;

        public static RequestContext DeleteLibrary()
        {
            using var context = new DictDbContext();
            try
            {
                if (context.Words.Any())
                {
                    context.Words.RemoveRange(context.Words);
                    context.SaveChanges();
                    _wordlist.Clear();
                    return new RequestContext("Словарь очищен", false);
                }
                else
                {
                    return new RequestContext("Очищение словаря не произведено, поскольку словарь пуст", true);
                }
            }
            catch (Exception e)
            {
                return new RequestContext("Exception in DeleteLibrary(): " + e.Message, true);
            }
        }

        public static RequestContext GetFilteredList(string wrdBgn, int wrdCnt)
        {
            if(wrdBgn.Contains(" ")) return new RequestContext("Введенное значение содержит недопустимые символы",true);
            if (!(_wordlist.Count > 0)) return new RequestContext("Словарь пуст или не создан", true);
            var temp = _wordlist.Where(x => x.word.ToLower().StartsWith(wrdBgn.ToLower()))
                .OrderByDescending(x => x.frequency).ThenBy(w => w.word).Take(wrdCnt).ToList();
            
            return new RequestContext("",false,temp);
        }

        public static RequestContext Initialize()
        {
            using var context = new DictDbContext();
            try
            {
                _wordlist = context.Words.ToList();
            }
            catch (Exception e)
            {
                return new RequestContext("Exception in Initialize():  "+ e.Message,true);
            }
            return new RequestContext();
        }



    }
}
