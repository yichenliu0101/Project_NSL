﻿using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Text;
using XAct;
using XAct.Library.Settings;

namespace Nsl_Core.Models.Infra.RandomGenerate
{
    public class RandomGenerator
    {
        private Random _rand;
        private readonly NSL_DBContext _db;
        private readonly HashPassword _hp;
        private readonly IConfiguration _config;
        private readonly SqlConnection _conn;

        private readonly string[] _firstName = { "趙", "錢", "孫", "李", "周", "吳", "鄭", "王", "馮", "陳", "褚", "衛", "蔣", "沈", "韓", "楊", "朱", "秦", "尤", "許", "何", "呂", "施", "張", "孔", "曹", "嚴", "華", "金", "魏", "陶", "姜", "戚", "謝", "鄒", "喻", "柏", "水", "窦", "章", "雲", "蘇", "潘", "葛", "奚", "范", "彭", "郎", "魯", "韋", "昌", "馬", "苗", "鳳", "花", "方", "俞", "任", "袁", "柳", "酆", "鮑", "史", "唐", "費", "廉", "岑", "薛", "雷", "賀", "倪", "湯", "滕", "殷", "羅", "畢", "郝", "鄔", "安", "常", "樂", "于", "時", "傅", "皮", "卞", "齊", "康", "伍", "余", "元", "卜", "顧", "孟", "平", "黃", "和", "穆", "蕭", "尹" };
        private readonly string[] _lastName = { "怡君", "雅婷", "欣怡", "雅雯", "家豪", "怡婷", "宗翰", "雅惠", "志豪", "心怡", "建宏", "佳蓉", "佩珊", "靜怡", "志偉", "雅玲", "佩君", " 俊宏", "佳穎", "怡伶", "婉婷", "俊傑", "郁婷", "怡如", "鈺婷", "靜宜", "彥廷", "冠宇", "佳玲", "詩婷", "家瑋", "承翰", "詩涵", "佳慧", " 惠雯", "宜君", "雅琪", "雅文", "柏翰", "韻如", "思穎", "俊賢", "玉婷", "淑芬", "琬婷", "家銘", "怡靜", "冠廷", "雅萍", "怡萱", "信宏", " 婷婷", "惠婷", "淑娟", "馨儀", "威廷", "雅慧", "淑惠", "佩蓉", "哲瑋", "智偉", "淑婷", "宜芳", "佳樺", "珮瑜", "嘉玲", "依婷", "雅芳", " 欣儀", "慧君", "芳瑜", "俊豪", "宗憲", "哲維", "志宏", "家瑜", "雅涵", "宜靜", "筱婷", "佳琪", "怡文", "淑君", "郁雯", "冠宏", "士豪", " 惠君", "家榮", "嘉宏", "偉倫", "雅筑", "怡潔", "慧玲", "佩玲", "欣穎", "建志", "惠如", "雅君", "明哲", "怡安", "孟儒", "于婷", "俊宇", " 美玲", "欣宜", "俊廷", "志鴻", "彥君", "宗霖", "芳儀", "俊毅", "怡慧", "瑋婷", "佩璇", "美君", "珮君", "建良", "政宏", "建銘", "柏宏", " 志強", "雅琳", "佳雯", "惠玲", "仁傑", "書豪", "志銘", "淑玲", "智凱", "盈君", "思妤", "佳霖", "士傑", "智翔", "建宇", "婉如", "淑萍", " 子豪", "偉哲", "凱翔", "文傑", "建勳", "博文", "筱涵", "淑華", "彥宏", "郁涵", "佳欣", "志遠", "怡璇", "嘉慧", "佳伶", "宇軒", "嘉文", " 玉玲", "俊凱", "思婷", "千惠", "雅芬", "建豪", "莉婷", "立偉", "志明", "怡菁", "淑貞", "靜雯", "家宏", "淑慧", "明宏", "怡芳", "舒婷", " 雅茹", "書瑋", "俊佑", "冠霖", "怡雯", "俊銘", "建智", "雅如", "哲宇", "佩穎", "宜蓁", "姿吟", "宜珊", "家維", "柏勳", "凱文", "佩芬", " 建文", "明翰", "怡秀", "惠文", "佳容", "人豪", "宗穎", "筱雯", "婉君", "雅鈴", "智傑", "佳怡", "凱婷", "文彥", "世偉", "俊良", "俊彥", " 欣樺", "彥儒", "育誠", "蕙如", "文豪", "瓊文", "伊婷", "俊瑋", "思涵", "哲豪", "嘉琪", "芳如", "姿君", "家偉", "姿穎", "佩樺", "慧如", " 聖文", "文馨", "郁文", "慧雯", "秀玲", "怡欣", "嘉偉", "怡均", "馨慧", "婉瑜", "英傑", "佳君", "怡萍", "靜儀", "美惠", "彥霖", "振宇", " 政翰", "家慧", "佳芳", "文彬", "佳瑩", "宜臻", "俊男", "明憲", "柏毅", "家慶", "韋廷", "姿瑩", "建廷", "姿伶", "美慧", "佩怡", "安琪", " 佳靜", "慧娟", "欣潔", "鎮宇", "柏廷", "千慧", "盈如", "珮如", "慧婷", "志瑋", "子翔", "昱廷", "淑芳", "建成", "柏宇", "宜庭", "佳惠", " 靖雯", "慧珊", "威志", "彥良", "志成", "曉雯", "佳宏", "建中", "維倫", "子軒", "承恩", "俊德", "凱傑", "宗賢", "宜婷", "彥伶", "惠茹", " 建安", "俊霖", "哲銘", "文君", "育如", "家齊", "志文", "珮綺", "怡臻", "建霖", "佩芳", "孟君", "宜真", "聖傑", "庭瑋", "乃文", "君豪", " 玉如", "孟穎", "淑敏", "威宇", "秋萍", "俊達", "宜樺", "美芳", "佳儀", "偉誠", "可欣", "嘉鴻", "文凱", "家華", "世豪", "志傑", "世傑", " 舒涵", "文婷", "志忠", "姵君", "雯婷", "雅嵐", "秀娟", "淑媛", "家弘", "佩宜", "政達", "政勳", "佩真", "嘉惠", "意婷", "世昌", "育賢", " 宗儒", "慧敏", "怡蓁", "宜潔", "郁芬", "冠伶", "佩娟", "佳燕", "麗君", "孟潔", "珮雯", "惠萍", "姿蓉", "淑雯", "宛蓉", "雅菁", "瓊慧", " 惠敏", "世杰", "珮珊", "冠穎", "嘉雯", "靜芳", "國豪", "怡廷", "志賢", "孟哲", "靜如", "正偉", "佩琪", "毓婷", "明勳", "佩瑜", "曉君", " 依潔", "佳琳", "育瑋", "明慧", "俊偉", "怡樺", "美娟", "子傑", "志龍", "宛儒", "玉芳", "冠儒", "孟翰", "偉翔", "怡芬", "逸群", "奕廷", " 家祥", "柏豪", "博仁", "怡真", "念慈", "孟勳", "柏翔", "惠娟", "玫君", "國維", "竹君", "美如", "于庭", "彥翔", "雅淳", "偉豪", "曉薇", " 佳芬", "家賢", "韋伶", "宛真", "佩儀", "佳芸", "曉婷", "瑋倫", "湘婷", "鈺雯", "孟軒", "麗娟", "珮甄", "維哲", "佳儒", "偉傑", "于珊", " 莉雯", "宗佑", "淑真", "智仁", "偉銘", "佩如", "佳宜", "嘉豪", "思潔", "志維", "仁豪", "柏均", "玉珊", "怡華", "瑋玲", "慧萍", "柏鈞", " 麗雯", "政憲", "俊諺", "耀文", "家綺", "建華", "孟璇", "姿儀", "欣蓉", "欣瑜", "彥豪", "佩雯", "巧玲", "佳勳", "智文", "韻婷", "柏霖", " 佳純", "聖凱", "建佑", "國華", "珮琪", "美伶", "蕙君", "佳真", "婉菁", "佳蓁", "雅欣", "宇翔", "彥文", "明峰", "婉玲", "玉芬", "育德", " 雅馨", "欣慧", "俊杰", "政霖", "思翰", "倩如", "佩芸", "彥志", "韋志", "建興", "志宇", "明達", "宗勳", "子揚", "士賢", "馨文", "凱鈞", " 柏凱", "盈潔", "彥甫", "曉玲", "雅晴", "佩瑩", "彥宇", "美華", "如君", "俊翰", "志誠", "文賢", "明倫", "怡蓉", "慧茹", "子涵", "裕翔", " 慧真", "冠傑", "思賢", "宏偉", "曉菁", "怡珊", "婉茹", "姿婷", "思瑩", "明志", "鴻文", "盈秀", "俊榮", "弘毅", "欣儒", "智豪", "郁珊", " 東霖", "佩儒", "于萱", "育民", "文琪", "明賢", "育菁", "瓊儀", "婕妤", "耀仁", "郁茹", "育銘", "彥均", "依璇", "政緯", "孟樺", "育慈", " 靜芬", "姿妤", "俊穎", "淑如", "思樺", "俊緯", "育瑄", "文宏", "玟君", "筱薇", "文正", "柏諺", "健銘", "依玲", "文", "俊翔", "俊仁", " 宜玲", "詩怡", "俐君", "家琪", "立婷", "彥勳", "士哲", "宗哲", "建瑋", "俊維", "冠文", "智鈞", "俊嘉", "郁翔", "書銘", "俊安", "志軒", " 冠華", "明儒", "俊明", "志杰", "佳瑜", "秀慧", "家欣", "國瑋", "景翔", "智堯", "怡穎", "柏瑋", "育萱", "健豪", "秉勳", "建德", "峻瑋", " 玉娟", "柏元", "明軒", "宗達", "家瑩", "佳璇", "宜萱", "致遠", "筱芸", "淑卿", "明芳", "曉萍", "子瑜", "香君", "思吟", "鈺涵", "育霖", " 耀德", "彥銘", "冠中", "怡瑩", "詩雅", "宜欣", "冠豪", "亭君", "峻豪", "怡瑄", "志祥", "志峰", "家緯", "怡嘉", "筱君", "逸軒", "依萍", " 岳霖", "逸凡", "玉萍", "慧芳", "貞儀", "宛臻", "哲緯", "書維", "俊逸", "淑珍", "明潔", "婉甄", "雨潔", "慧珍", "思慧", "偉智", "瑋", "曉 琪", "慧貞", "心瑜", "冠樺", "麗如", "振豪", "昱宏", "立群", "郁芳", "雅云", "政哲", "哲偉", "靖雅", "國榮", "佩伶", "彥如", "怡禎", "秀 雯", "俊吉", "欣妤", "宏達", "佳倫", "雅鈞", "大為", "玟伶", "孟珊", "宜芬", "志榮", "婷", "俊成", "柏青", "文怡", "佳臻", "曉慧", "佳吟", "政賢", "庭瑜", "宜璇", "玉雯", "文欣", "文斌", "佳珍", "嘉琳", "彥鈞", "郁欣", "珮慈", "馨瑩", "柏安", "彥婷", "永昌", "宗樺", "哲民", "育豪", "曉芬", "怡妏", "智超", "明德", "敏華", "耀中", "明輝", "啟宏", "建邦", "建明", "佩欣", "雅莉", "易霖", "宜穎", "國峰", "淑怡", "秀如", "靜慧", "尚儒", "惠琪", "秋燕", "雅娟", "嘉瑩", "志翔", "秉翰", "俊儒", "文祥", "國偉", "寧", "俊憲", "凱元", "慧文", "晏如", "志 仁", "靜茹", "美琪", "嘉祥", "安妮", "惠珍", "博凱", "冠儀", "鳳儀", "家興", "博元", "明杰", "俊雄", " 家禎", "維真", "雅雲", "偉志", "瑋珊", "淑美", "詩穎", "珮瑄", "婕", "宗祐", "志揚", "彥博", "政諺", "裕仁", "信豪", "銘鴻", "文華", "俊龍", "昱翔", "建璋", "凱琳", "士弘", "欣如", "力瑋", "詠翔", "奕君", "偉成", "博鈞", "建榮", "宏仁", "金龍", "哲安", "惠菁", "瑞鴻", "冠甫", "怡玲", "佩吟", "詩雯", "大維", "嘉駿", "惠珊", "政儒", "柏志", "智強", "永祥", "昆霖", "漢威", "文龍", "嘉倫", "勝傑", "育正", "俐伶", "建彰", "育廷", "佳琦", "怡貞", "家誠", "凱雯", "冠良", "家寧", "彥伯", "于真", "淑雅", "晉嘉", "婉柔", "佩萱", "景文", "嘉芸", "思齊", "虹君", "孟修", "玉菁", "怡儒", " 秀芬", "宜儒", "懿萱", "宏銘", "奕安", "怡青", "偉民", "冠瑋", "秀婷", "家儀", "承勳", "依珊", "建忠", "書瑜", "建男", "婉琳", "妍伶", " 心儀", "宏文", "瑜珊", "信良", "盈盈", "鼎鈞", "孟婷", "佩容", "世明", "宜珍", "致豪", "淑儀", "子芸", "美吟", "孟蓉", "智勇", "淑鈴", " 玉潔", "奕辰", "育儒", "佳鴻", "巧雯", "彥傑", "明宗", "嘉芳", "欣蓓", "士銘", "嘉仁", "詩芸", "怡珍", "佩茹", "婉萍", "宗毅", "世賢", " 俞君", "彥佑", "柏仁", "柏儒", "麗婷", "明璋", "建樺", "宜軒", "郁仁", "建緯", "銘宏", "孟娟", "志堅", "珊珊", "宜慧", "佳盈", "筱筠", " 佳翰", "育嘉", "仁宏", "逸婷", "信安", "士軒", "思嘉", "珮茹", "惠芳", "文政", "文心", "宛君", "雅琴", "英哲", "瑞祥", "志雄", "慧怡", " 嘉慶", "建甫", "奕翔", "政穎", "書賢", "明君", "怡寧", "孟芳", "傑", "凱倫", "筱筑", "宛玲", "允中", "玉琳", "啟銘", "美儀", "育婷", "佳 音", "佳恩", "明修", "震宇", "宜蓉", "威任", "昱辰", "宏儒", "政偉", "育維", "自強", "明穎", "麗華", "文志", "婉真", "竣傑", "柏辰", "哲 嘉", "偉婷", "嘉容", "宛庭", "仲豪", "家源", "廷宇", "玉華", "聖翔", "品潔", "淑菁", "子維", "盈瑩", "曉嵐", "君儀", "子婷", "柏成", "宛 婷", "鈺翔", "婉琪", "昱豪", "惠美", "志鵬", "聖哲", "亞璇", "明鴻", "維仁", "柏村", "銘哲", "珮玲", "大鈞", "薇如", "智瑋", "依靜", "耀 賢", "美秀", "曉涵", "志平", "育仁", "韋翔", "秀芳", "柏賢", "冠銘", "晉瑋", "文瑜", "家駿", "彥蓉", "俊儀", "振瑋", "雅琦", "姿慧", "宛 諭", "宜娟", "柏任", "鈺茹", "鈺珊", "冠群", "書婷", "佳瑋", "信傑", "宇婷", "慧慈", "韻竹" };
        private readonly string[] _emailcom = { "@gmail.com", "@yahoo.com.tw", "@msn.com", "@hotmail.com", "@live.com", "@qq.com", "@ispan.com.tw", "@aol.com", "@ask.com" };
        //private readonly string[] _cityId = { "臺北市文山區", "臺中市霧峰區", "雲林縣東勢鄉", "花蓮縣吉安鄉", "南投縣水里鄉", "屏東縣萬丹鄉", "臺中市沙鹿區", "嘉義縣民雄鄉", "高雄市前鎮區", "臺中市豐原區", "新北市新莊區", "新北市新莊區", "臺南市中西區", "桃園市中壢區", "臺南市東區", "臺中市北屯區", "苗栗市", "南投縣竹山鎮", "臺北市大安區", "宜蘭縣冬山鄉", "屏東縣南州鄉", "新竹縣新埔鎮", "宜蘭縣羅東鎮", "臺南市安南區", "花蓮縣壽豐鄉", "彰化縣伸港鄉", "彰化縣伸港鄉", };
        //private readonly string[] _street = { "忠孝路", "仁愛路", "信義路", "和平路", "民族路", "民權路", "民生路", "光復路", "復興路", "建國路", "中山路", "中正路", "藍藍路", "朱崙街", "天玉街", "昌隆街", "天堂路", "垂楊路", "館前路", "天泉路", "寶慶路" };
        //private readonly string[] _zhNumber = { "一", "二", "三", "四", "五", "六", "七" };
        //private readonly string[] _petName = { "米漿", "奶茶", "奶油", "布丁", "橘子", "可樂", "土豆", "麻糬", "巧克力", "牛奶", "咖啡", "桃子", "海苔", "黑糖", "餅乾", "饅頭", "沙西米", "哇沙米", "維士比", "小花", "黑皮", "斑斑", "小牛", "湯圓", "脆片", "老闆", "胖胖", "丁丁", "黑", "白", "花", "宵夜", "花捲", "牛奶", "辛巴", "金城武", "摩卡", "半糖", "拿鐵", "饅頭", "丸子", "月餅", "玉米", "香菇", "桂圓", "荔枝", "紅豆", "皮蛋", "可可", "黑糖", "芋頭", "抹茶", "可樂", "橘子" };
        public RandomGenerator(NSL_DBContext db, IConfiguration config)
        {
            _rand = new Random(Guid.NewGuid().GetHashCode());
            _db = db;
            _hp = new HashPassword(_db);
            _config = config;
            _conn= new SqlConnection(_config.GetConnectionString("NSLDbContext"));
        }
        public string RandomName()
        {
            string name = _firstName[_rand.Next(_firstName.Length)];
            name += _lastName[_rand.Next(_lastName.Length)];
            return name;
        }

        public DateTime RandomBirthDate(int minAge = 10, int maxAge = 60)
        {
            if (minAge > maxAge)
            {
                throw new ArgumentOutOfRangeException("前項為最小年齡，不可大於後項最大年齡");
            }

            DateTime dt = DateTime.Now.Date;
            dt = dt.AddYears(-maxAge);
            dt = dt.AddDays(_rand.Next((maxAge - minAge) * 365));
            return dt;
        }

        public DateTime RandomDateBetweenDays(int minDay = -150, int maxDay = -3)
        {
            if (minDay > maxDay)
            {
                throw new ArgumentOutOfRangeException("前項時間不可大於後項");
            }
            // 3600 * 24 

            DateTime dt = DateTime.Now.AddDays(minDay);
            dt = dt.AddDays(_rand.Next(maxDay - minDay)).AddSeconds(_rand.Next(86400));
            return dt;
        }


        public string RandomEnString(int min = 5, int max = 15)
        {
            int length = _rand.Next(min, max);
            string str = string.Empty;
            str += (char)('A' + _rand.Next(26));
            for (int i = 0; i < length - 1; i++)
            {
                str += (char)('a' + _rand.Next(26));
            }
            return str;
        }

        public string RandomSalt(int min = 10, int max = 15)
        {
            int length = _rand.Next(min, max);
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str += (char)('!' + _rand.Next(94));
            }
            return str;
        }

        public string RandomEmail()
        {
            string str = RandomEnString();
            str += _emailcom[_rand.Next(_emailcom.Length)];

            return str;
        }

        public string RandomPhone()
        {
            string str = "09"
                + _rand.Next(10, 90).ToString("d2")     
                + _rand.Next(1000).ToString("d3")
                + _rand.Next(1000).ToString("d3");

            return str;
        }

        public int RandomCityId(out int areaId, int min = 1, int max = 22)
        {
            //生成隨機cityId並儲存
            //使用存取的cityId去生成areaId
            //LINQ _db.Areas.Where(x=>x.CityId == 隨機生成的CityId).toList
            //再用Random.Next產生隨機的areaId

            int cityId = _rand.Next(min, max);
            NSL_DBContext db = new();
            var areaList = db.Areas.Where(o => o.CityId == cityId).OrderBy(x => x.Id).ToList();
            areaId = _rand.Next(areaList[0].Id, areaList[areaList.Count - 1].Id);

            return cityId;
        }

		public string GenerateRandomPassword(int length, out string hashPassword, out string salt)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			StringBuilder passwordBuilder = new StringBuilder();

			for (int i = 0; i < length; i++)
			{
				char randomChar = chars[random.Next(chars.Length)];
				passwordBuilder.Append(randomChar);
			}

            hashPassword = _hp.GenerateHashPassword(passwordBuilder.ToString(), out salt);
			return passwordBuilder.ToString();

		}
	

	public int RandomIntBetween(int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException("前項為最小值，不可大於後項");
            }
            return _rand.Next(start, end + 1);
        }

        public T RandomFrom<T>(IEnumerable<T> objs)
        {
            if (objs == null)
            {
                throw new ArgumentOutOfRangeException("傳入集合為null");
            }
            int count = objs.Count();
            var arr = objs.ToArray();

            return arr[_rand.Next(count)];
        }

        public IEnumerable<T> RandomCollectionFrom<T>(IEnumerable<T> objs, int amount)
        {
            if (objs == null)
            {
                throw new ArgumentOutOfRangeException("傳入集合為null");
            }
            int count = objs.Count();
            if (count < amount) amount = count;

            var newlist = new List<T>();

            while (amount > 0)
            {
                var obj = RandomFrom(objs);
                if (!newlist.Contains(obj))
                {
                    newlist.Add(obj);
                    amount--;
                }
            }
            return newlist;
        }


        public bool RandomBool()
        {
            return _rand.Next(2) == 0;
        }

        public bool RandomChance(int percent)
        {
            if (percent < 0) percent = 0;
            if (percent > 100) percent = 100;

            return percent > _rand.Next(0, 100);
        }

        public int RandomIntByWeight(params int[] weights)
        {
            if (weights == null || weights.Length == 0) return 0;

            int sum = weights.Sum();

            int rand = _rand.Next(sum);

            sum = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                if (sum > rand) return i;
            }
            return 0;
        }

        private readonly string[] _MerchandiseTitle0 = { "美國進口", "英國進口", "台灣品牌", "日本原裝進口", "芬蘭進口", "紐西蘭特產", "法國原裝", "德國引進", "花蓮進口" };
        private readonly string[] _MerchandiseTitle2 = { "精裝版", "特仕版", "豪華", "經典款", "經濟版" };

        private readonly string[][] _MerchandiseItem = {
            new string[] { "貓罐頭", "狗罐頭", "寵物零食", "牧草", "綜合瓜子"}, //寵物食品
            new string[] { "魚油", "維他命", "潔牙骨" }, //營養保健
            new string[] { "木屑", "貓砂", "尿布墊", "鼠砂"}, //墊料耗材
            new string[] { "寵物貓專用沐浴乳", "寵物犬專用沐浴乳", "寵物用濕紙巾"}, //寵物清潔
            new string[] { "毛梳", "指甲剪", "餵水器" }, //器材工具
            new string[] { "貓抓板", "骨頭玩具", "蘿蔔娃娃", "滾輪"}, //寵物玩具
            new string[] {"狗狗衣服", "貓咪頭套", "兔子領結" }, //飾品服裝
            new string[] { "貓窩", "狗窩籃子", "鼠籠", "兔籠" },//家居小窩
            new string[] {"牽繩", "貓用外出背包", "寵物箱" }, //外出用品
        };


        public string GetMerchandiseName(int categoryIndex)
        {
            string name = (RandomChance(80) ? _MerchandiseTitle0[_rand.Next(_MerchandiseTitle0.Length)] : "")
                + (RandomChance(90) ? _MerchandiseTitle2[_rand.Next(_MerchandiseTitle2.Length)] : "")
                + _MerchandiseItem[categoryIndex][_rand.Next(_MerchandiseItem[categoryIndex].Length)];

            return name;
        }

        private readonly string[] _Specs = { "(特大)", "(大)", "(中)", "(小)", "(一百份裝)", "(十二份裝)", "(單份)", "(五份裝)", "(紅色)", "(綠色)", "(彩虹色)" };
        public string[] GetSpecName(int count)
        {
            return RandomCollectionFrom(_Specs, count).ToArray();
        }



        private readonly string[] _nouns = { "bird", "clock", "boy", "plastic", "duck", "teacher", "old lady", "professor", "hamster", "dog" };
        private readonly string[] _verbs = { "kicked", "ran", "flew", "dodged", "sliced", "rolled", "died", "breathed", "slept", "killed" };
        private readonly string[] _adjectives = { "beautiful", "lazy", "professional", "lovely", "dumb", "rough", "soft", "hot", "vibrating", "slimy" };
        private readonly string[] _adverbs = { "slowly", "elegantly", "precisely", "quickly", "sadly", "humbly", "proudly", "shockingly", "calmly", "passionately" };
        private readonly string[] _preposition = { "down", "into", "up", "on", "upon", "below", "above", "through", "across", "towards" };

        public string RandomSentance()
        {
            int rand1 = _rand.Next(10);
            int rand2 = _rand.Next(10);
            int rand3 = _rand.Next(10);
            int rand4 = _rand.Next(10);
            int rand5 = _rand.Next(10);
            int rand6 = _rand.Next(10);

            var content = "The " + _adjectives[rand1] + " "
                + _nouns[rand2] + " " + _adverbs[rand3] + " "
                + _verbs[rand4] + " because some " + _nouns[rand1]
                + " " + _adverbs[rand1] + " " + _verbs[rand1] + " "
                + _preposition[rand1] + " a " + _adjectives[rand2] + " "
                + _nouns[rand5] + " which, became a " + _adjectives[rand3] + ", "
                + _adjectives[rand4] + " " + _nouns[rand6] + ".";

            return content;
        }

        public void RandomTutorPeriod()
        {
            int count = 0;
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    while (count < 1000)
                    {
                        #region 宣告隨機數
                        int teacherId = _db.TeachersResume.Where(x => x.Introduction != null && x.Price != null && x.Title != null).ToList()
                            .OrderBy(x => Guid.NewGuid().GetHashCode()).First().TeacherId;
                        var startDate = new DateTime(2023, 7, 1);
                        var endDate = new DateTime(2023, 8, 24);
                        int hours = _rand.Next(0, 24);
                        int days = _rand.Next((endDate - startDate).Days);
                        var date = startDate.AddDays(days).AddHours(hours);

                        int memberId = _db.Members.Where(x => x.Role == 1).ToList()
                                            .OrderBy(x => Guid.NewGuid().GetHashCode()).First().Id;
                        #endregion

                        var teacherPeriod = new TeachersRealTutorPeriods()
                        {
                            TeacherId = teacherId,
                            TutorStartTime = date,
                            Status = true
                        };
                        _db.TeachersRealTutorPeriods.Add(teacherPeriod);
                        _db.SaveChanges();

                        var memberTutorPeriod = new MembersTutorRecords()
                        {
                            MemberId = memberId,
                            TeacherTutorPeriodId = teacherPeriod.Id,
                            Status = true
                        };
                        _db.MembersTutorRecords.Add(memberTutorPeriod);
                        _db.SaveChanges();

                        count++;
                    }
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"{ex.Message}");
                }
            }
        }

        public void RandomComment()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>
            {
                { 4, "老師教學很優秀，會繼續購買老師的課程"},
                { 5,"老師的教學十分充足完善，上完有真正學到東西"}
            };
            var query = (from mtr in _db.MembersTutorRecords
                        join c in _db.Comments on mtr.Id equals c.MemberTutorRecordId into cc
                        from ci in cc.DefaultIfEmpty()
                        where mtr.Status == true && ci.Satisfaction == null
                        select mtr.Id).ToList().OrderBy(x => Guid.NewGuid().GetHashCode());

            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in query)
                    {
                        int randomSatis = _rand.Next(4, 6);
                        string comment = dict[randomSatis];
                        var entity = new Comments()
                        {
                            MemberTutorRecordId = item,
                            Satisfaction = randomSatis,
                            CommentContent = comment,
                        };

                        _db.Comments.Add(entity);
                        _db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            }
            
        }
    }
}
