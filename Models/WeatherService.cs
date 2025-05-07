using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using WpfAdvancedBindingDemo.Commands;

namespace WpfAdvancedBindingDemo.Models
{
    // 天气服务类
    public class WeatherService : INotifyPropertyChanged
    {
        private string? _weatherForecast;
        private string _location = "Beijing";
        private string _weatherComCityCode = "101010100"; // 默认北京
        private readonly HttpClient _httpClient;
        private bool _isLocationValid = true;
        
        // 英文天气描述转中文映射表
        private static readonly Dictionary<string, string> WeatherTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Clear", "晴朗" },
            { "Sunny", "晴天" },
            { "Partly cloudy", "局部多云" },
            { "Cloudy", "多云" },
            { "Overcast", "阴天" },
            { "Mist", "薄雾" },
            { "Fog", "雾" },
            { "Freezing fog", "冻雾" },
            { "Patchy rain possible", "可能有零星小雨" },
            { "Patchy snow possible", "可能有零星小雪" },
            { "Patchy sleet possible", "可能有零星雨夹雪" },
            { "Patchy freezing drizzle possible", "可能有零星冻毛毛雨" },
            { "Thundery outbreaks possible", "可能有雷雨" },
            { "Blowing snow", "吹雪" },
            { "Blizzard", "暴风雪" },
            { "Rain", "雨" },
            { "Heavy rain", "大雨" },
            { "Light rain", "小雨" },
            { "Moderate rain", "中雨" },
            { "Light rain shower", "小阵雨" },
            { "Moderate or heavy rain shower", "中到大阵雨" },
            { "Torrential rain shower", "暴雨" },
            { "Sleet", "雨夹雪" },
            { "Light sleet", "小雨夹雪" },
            { "Moderate or heavy sleet", "中到大雨夹雪" },
            { "Light snow", "小雪" },
            { "Moderate snow", "中雪" },
            { "Heavy snow", "大雪" },
            { "Ice pellets", "冰粒" },
            { "Light showers of ice pellets", "小冰粒阵雨" },
            { "Moderate or heavy showers of ice pellets", "中到大冰粒阵雨" },
            { "Patchy light rain with thunder", "雷阵雨" },
            { "Moderate or heavy rain with thunder", "雷阵雨伴有中到大雨" },
            { "Patchy light snow with thunder", "雷雪" },
            { "Moderate or heavy snow with thunder", "雷雪伴有中到大雪" },
            { "Rain with thunderstorm", "雷暴雨" },
            { "Drizzle", "毛毛雨" },
            { "Thunderstorm", "雷暴" }
        };
        
        // 省份/省会/直辖市/常用别名 到 天气网城市代码映射
        private static readonly Dictionary<string, string> CityCodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // 北京
            { "beijing", "101010100" }, { "北京", "101010100" },
            // 天津
            { "tianjin", "101030100" }, { "天津", "101030100" },
            // 上海
            { "shanghai", "101020100" }, { "上海", "101020100" },
            // 重庆
            { "chongqing", "101040100" }, { "重庆", "101040100" },
            // 河北 石家庄
           { "河北", "101090101" }, { "hebei", "101090101" }, { "shijiazhuang", "101090101" }, { "石家庄", "101090101" },
            // 山西 太原
            { "山西", "101100101" },{ "shanxi", "101100101" }, { "taiyuan", "101100101" }, { "太原", "101100101" },
            // 辽宁 沈阳
           { "辽宁", "101070101" }, { "liaoning", "101070101" }, { "shenyang", "101070101" }, { "沈阳", "101070101" },
            // 吉林 长春
          { "吉林", "101060101" },  { "jilin", "101060101" }, { "changchun", "101060101" }, { "长春", "101060101" },
            // 黑龙江 哈尔滨
            { "黑龙江", "101050101" }, { "heilongjiang", "101050101" }, { "haerbin", "101050101" }, { "哈尔滨", "101050101" },
            // 江苏 南京
          { "江苏", "101190101" },  { "jiangsu", "101190101" }, { "nanjing", "101190101" }, { "南京", "101190101" },
            // 浙江 杭州
           { "浙江", "101210101" }, { "zhejiang", "101210101" }, { "hangzhou", "101210101" }, { "杭州", "101210101" },
            // 安徽 合肥
           { "安徽", "101220101" }, { "anhui", "101220101" }, { "hefei", "101220101" }, { "合肥", "101220101" },
            // 福建 福州
           { "福建", "101230101" }, { "fujian", "101230101" }, { "fuzhou", "101230101" }, { "福州", "101230101" },
            // 江西 南昌
             { "江西", "101240101" }, { "jiangxi", "101240101" }, { "nanchang", "101240101" }, { "南昌", "101240101" },
            // 山东 济南
             { "山东", "101120101" },{ "shandong", "101120101" }, { "jinan", "101120101" }, { "济南", "101120101" },
            // 河南 郑州
           { "河南", "101180101" }, { "henan", "101180101" }, { "zhengzhou", "101180101" }, { "郑州", "101180101" },
            // 湖北 武汉
          { "湖北", "101200101" },  { "hubei", "101200101" }, { "wuhan", "101200101" }, { "武汉", "101200101" },
            // 湖南 长沙
            { "湖南", "101250101" }, { "hunan", "101250101" }, { "changsha", "101250101" }, { "长沙", "101250101" },
            // 广东 广州
          { "广东", "101280101" },   { "guangdong", "101280101" }, { "guangzhou", "101280101" }, { "广州", "101280101" },
            // 广西 南宁
          { "广西", "101300101" },   { "guangxi", "101300101" }, { "nanning", "101300101" }, { "南宁", "101300101" },
            // 海南 海口
          { "海南", "101310101" },   { "hainan", "101310101" }, { "haikou", "101310101" }, { "海口", "101310101" },
            // 四川 成都
           { "四川", "101270101" },  { "sichuan", "101270101" }, { "chengdu", "101270101" }, { "成都", "101270101" },
            // 贵州 贵阳
           { "贵州", "101260101" }, { "guizhou", "101260101" }, { "guiyang", "101260101" }, { "贵阳", "101260101" },
            // 云南 昆明
            { "云南", "101290101" },{ "yunnan", "101290101" }, { "kunming", "101290101" }, { "昆明", "101290101" },
            // 陕西 西安
           { "陕西", "101110101" },   { "shanxi2", "101110101" }, { "shaanxi", "101110101" }, { "xian", "101110101" }, { "西安", "101110101" },
            // 甘肃 兰州
           { "甘肃", "101160101" },  { "gansu", "101160101" }, { "lanzhou", "101160101" }, { "兰州", "101160101" },
            // 青海 西宁
          { "青海", "101150101" },  { "qinghai", "101150101" }, { "xining", "101150101" }, { "西宁", "101150101" },
            // 宁夏 银川
           { "宁夏", "101170101" },  { "ningxia", "101170101" }, { "yinchuan", "101170101" }, { "银川", "101170101" },
            // 新疆 乌鲁木齐
            { "新疆", "101130101" }, { "xinjiang", "101130101" }, { "wulumuqi", "101130101" }, { "乌鲁木齐", "101130101" },
            // 西藏 拉萨
         { "西藏", "101140101" },    { "xizang", "101140101" }, { "lasa", "101140101" }, { "拉萨", "101140101" },
            // 香港
           { "xianggang", "101320101" },  { "hongkong", "101320101" }, { "香港", "101320101" },
            // 澳门
             { "aomen", "101330101" }, { "macao", "101330101" }, { "澳门", "101330101" },
            // 台湾 台北
            { "台湾", "101340101" }, { "taiwan", "101340101" }, { "taipei", "101340101" }, { "台北", "101340101" }
        };
        
        // 实现INotifyPropertyChanged接口
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        // 天气预报属性
        public string? WeatherForecast
        {
            get => _weatherForecast;
            private set
            {
                if (_weatherForecast != value)
                {
                    _weatherForecast = value;
                    OnPropertyChanged();
                }
            }
        }
        
        // 位置属性
        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                    // 位置变更后更新城市代码
                    UpdateWeatherComCityCode(value);
                }
            }
        }
        
        // 中国天气网城市代码
        public string WeatherComCityCode
        {
            get => _weatherComCityCode;
            set
            {
                if (_weatherComCityCode != value)
                {
                    _weatherComCityCode = value;
                    OnPropertyChanged();
                }
            }
        }
        
        // 将英文天气描述转换为中文
        private string TranslateWeather(string englishWeather)
        {
            // 尝试直接查找完整的天气描述
            if (WeatherTranslations.TryGetValue(englishWeather.Trim(), out string? translation))
            {
                return translation;
            }
            
            // 如果完整描述没找到，尝试查找包含的部分词语
            foreach (var pair in WeatherTranslations)
            {
                if (englishWeather.Contains(pair.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Value;
                }
            }
            
            // 如果找不到匹配，返回原文
            return englishWeather;
        }
        
        // 刷新天气命令
        public ICommand RefreshWeatherCommand => new RelayCommand(async () => await FetchWeatherAsync());
        
        // 构造函数
        public WeatherService()
        {
            // 初始时为空，触发PriorityBinding回退到下一级
            _weatherForecast = null;
            _httpClient = new HttpClient();
        }
        
        // 构造函数(带位置参数)
        public WeatherService(string location)
        {
            _weatherForecast = null;
            _location = location;
            _httpClient = new HttpClient();
            UpdateWeatherComCityCode(location);
        }
        
        // 更新天气网城市代码
        private void UpdateWeatherComCityCode(string location)
        {
            if (CityCodeMap.TryGetValue(location.ToLower(), out var code))
            {
                _weatherComCityCode = code;
                _isLocationValid = true;
            }
            else
            {
                _weatherComCityCode = "101010100"; // 默认北京
                _isLocationValid = false;
            }
        }
        
        // 从天气网站抓取真实天气数据
        private async Task FetchWeatherAsync()
        {
            if (!_isLocationValid)
            {
                WeatherForecast = "未检索到相关位置信息，请重新输入";
                WeatherCache.Instance.CachedWeatherForecast = WeatherForecast;
                return;
            }
            // 设置为null触发绑定回退
            WeatherForecast = null;
            
            try
            {
                // 优先使用wttr.in获取天气数据（这个服务更可靠）
                await FetchWeatherFromWttrIn();
            }
            catch
            {
                try
                {
                    // 如果wttr.in失败，尝试使用天气网
                    await FetchWeatherFromWeatherCom();
                }
                catch (Exception innerEx)
                {
                    // 两种方式都失败，显示错误信息
                    WeatherForecast = $"获取天气数据失败: {innerEx.Message}";
                }
            }
        }
        
        // 使用wttr.in API获取天气
        private async Task FetchWeatherFromWttrIn()
        {
            // wttr.in支持中文和JSON格式
            string url = $"https://wttr.in/{_location}?format=j1&lang=zh";
            
            // 发送请求
            string jsonContent = await _httpClient.GetStringAsync(url);
            
            // 解析JSON
            JObject weatherData = JObject.Parse(jsonContent);
            
            // 提取天气信息 - 添加空检查
            string weather = "未知";
            string temperature = "未知";
            
            if (weatherData["current_condition"]?[0]?["weatherDesc"]?[0]?["value"] != null)
            {
                // 使用？.操作符安全访问，避免空引用
                var weatherValue = weatherData["current_condition"]?[0]?["weatherDesc"]?[0]?["value"];
                if (weatherValue != null)
                {
                    string englishWeather = weatherValue.ToString();
                    // 翻译成中文
                    weather = TranslateWeather(englishWeather);
                }
            }
            
            if (weatherData["current_condition"]?[0]?["temp_C"] != null)
            {
                // 使用？.操作符安全访问，避免空引用
                var tempValue = weatherData["current_condition"]?[0]?["temp_C"];
                if (tempValue != null)
                {
                    temperature = tempValue.ToString() + "°C";
                }
            }
            
            // 更新天气数据
            WeatherForecast = $"{Location}：{weather}，温度{temperature}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}更新";
            
            // 同时更新缓存
            WeatherCache.Instance.CachedWeatherForecast = WeatherForecast;
        }
        
        // 从中国天气网获取天气数据
        private async Task FetchWeatherFromWeatherCom()
        {
            // 使用城市代码获取天气页面URL
            string url = $"http://www.weather.com.cn/weather/{_weatherComCityCode}.shtml";
            
            // 发送请求获取HTML内容
            string htmlContent = await _httpClient.GetStringAsync(url);
            
            // 创建HtmlDocument并加载HTML内容
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            
            // 提取今天的天气信息
            // 注意：网站结构可能会变化，选择器可能需要调整
            var weatherNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class='t clearfix']/li[1]//p[@class='wea']");
            var tempNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class='t clearfix']/li[1]//p[@class='tem']");
            
            string weather = "未知";
            string temperature = "未知";
            
            if (weatherNode != null)
            {
                weather = weatherNode.InnerText.Trim();
            }
            
            if (tempNode != null)
            {
                temperature = tempNode.InnerText.Trim();
            }
            
            // 更新天气数据
            WeatherForecast = $"{Location}：{weather}，温度{temperature}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}更新";
            
            // 同时更新缓存
            WeatherCache.Instance.CachedWeatherForecast = WeatherForecast;
        }
    }
    
    // 天气缓存类
    public class WeatherCache : INotifyPropertyChanged
    {
        private static WeatherCache? _instance;
        public static WeatherCache Instance => _instance ??= new WeatherCache();
        
        private string? _cachedWeatherForecast;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string? CachedWeatherForecast
        {
            get => _cachedWeatherForecast;
            set
            {
                if (_cachedWeatherForecast != value)
                {
                    _cachedWeatherForecast = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private WeatherCache()
        {
            // 初始缓存数据
            _cachedWeatherForecast = "昨日天气：晴，温度24°C (缓存数据)";
        }
    }
} 