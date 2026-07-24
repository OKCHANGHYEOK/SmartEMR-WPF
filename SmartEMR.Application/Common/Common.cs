using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.SmartEMRRES;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Globalization;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartEMR.Application.Common;

public enum eSmartEMRLocation 
{ 
    CALENDAR = 0,
    DESK = 1,
    EXAM = 2,
    PAYMENT = 3,
    CRM = 4,
    CONFIG = 5
}

public enum eBirthType
{
    Year,
    Month,
    Day
}

public enum SaveMode
{
    SAVE,
    DELETE
}

public class Common
{
    public BrushConverter BrushConverter { get; } = new BrushConverter();

    private List<CommonCode> _arrCCC = new();
    public IReadOnlyList<CommonCode> arrCCC => _arrCCC.AsReadOnly();

    private Dictionary<(string? CCC_Cd, string? CCG_Cd, string? CCI_Cd), string?> _cccMapper = new();

    public async Task Initialize()
    {
        var retCCC = await SmartMVVM.DataStore.GetItems<CommonCode>(eAPI.CommonCode_GetCommonCode, new CommonCode());
        if (retCCC == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("CommonCode_GetCommonCode 조회에 실패했습니다.", NotificationType.Error);
            return;
        }

        _arrCCC = retCCC.ToList();
        _cccMapper = _arrCCC.ToDictionary(x => (x.CCC_Cd, x.CCG_Cd, x.CCI_Cd), x => x.CCI_Name);
    }

    public void DisposeControl(object? element)
    {
        if (element == null) return;

        if (element is IDisposable disposable)
        {
            disposable.Dispose(true);
        }

        if (element is DependencyObject obj)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            
            for (int i =0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);  

                DisposeControl(child);
            }
        }

        if (element is ViewLayout view)
        {
            foreach (var bindGrid in view.BindGrids)
            {
                bindGrid.BindGrid_BindClickEvent -= view.OnBindGrid_BindClick;
                bindGrid.BindGrid_BindItemChangedEvent -= view.OnBindGrid_BindItemChanged;
            }

            foreach (var dataGrid in view.DataGrids)
            {
                dataGrid.DataGrid_DataItemChangedEvent -= view.OnDataGrid_DataItemChanged;
                dataGrid.DataGrid_PopupMenuOpening -= view.OnDataGrid_PopupMenuOpening;
                dataGrid.DataGrid_PopupMenuItemClick -= view.OnDataGridPopupMenu_PopupMenuItemClicked;
            }

            SmartUI.Messenger.UnRegister(view);
        }
    }

    public IEnumerable<CommonCode> GetCommonCode(string CCC_Cd = "", string CCG_Cd = "", string CCI_Cd = "", bool isDefault = false, string defaultText = "전체")
    {
        List<CommonCode> retCCC = new();

        if (isDefault)
        {
            retCCC.Add(new CommonCode { CCI_Name = defaultText, CCI_Cd = ""});
        }

        IEnumerable<CommonCode>? targetItems = arrCCC;

        if (!string.IsNullOrWhiteSpace(CCC_Cd))
        {
            targetItems = targetItems.Where(x => x.CCC_Cd == CCC_Cd);
        }

        if (!string.IsNullOrWhiteSpace(CCG_Cd))
        {
            targetItems = targetItems.Where(x => x.CCG_Cd == CCG_Cd);
        }

        if (!string.IsNullOrWhiteSpace(CCI_Cd))
        {
            targetItems = targetItems.Where(x => x.CCI_Cd == CCI_Cd);
        }
        
        if (targetItems != null)
        {
            retCCC.AddRange(targetItems);
        }

        return retCCC.AsEnumerable();
    }

    public string? GetCommonCodeName(string CCC_Cd = "", string CCG_Cd = "", string CCI_Cd = "")
    {
        if (string.IsNullOrWhiteSpace(CCI_Cd)) return null;

        return _cccMapper.GetValueOrDefault((CCC_Cd, CCG_Cd, CCI_Cd), "");
    }

    public IEnumerable<object> GetBirth(eBirthType birthType)
    {
        List<object> arrBirth = new();

        var nowDT = DateTime.Now;

        int sValue = 0;
        int eValue = 0;

        if (birthType == eBirthType.Year)
        {

            sValue = nowDT.Year - 120;
            eValue = nowDT.Year;
        }
        else if (birthType == eBirthType.Month)
        {
            sValue = 1;
            eValue = 12;
        }
        else if (birthType == eBirthType.Day)
        {
            sValue = 1;
            eValue = 31;
        }

        for (int i = sValue; i <= eValue; i++)
        {
            arrBirth.Add(new
            {
                attrName = i,
                attrValue = i
            });
        }

        arrBirth.Reverse();

        return arrBirth.AsEnumerable();
    }

    public IEnumerable<object> GetTimesByInterval(int interval)
    {
        List<object> times = new();

        for (int i = 0; i < 24; i++)
        {
            string strAMPM = i < 12 ? "오전" : "오후";
            string strHH = (i % 12).ToString().PadLeft(2, '0');

            for (int j = 0; j < 60; j += interval)
            {
                string strMM = j.ToString().PadLeft(2, '0');
                string attrValue = i.ToString().PadLeft(2, '0') + ":" + strMM;
                string attrName = strAMPM + " " + attrValue;

                times.Add(new { attrName, attrValue });
            }
        }

        return times.AsEnumerable();
    }

    public string GetRoundUpTimeByInterval(DateTime dt, int interval)
    {
        int minutes = interval - dt.Minute % interval;
        DateTime newDT = dt.AddMinutes(minutes);

        return newDT.ToString("HH:mm");
    }

    public List<ReservationSlot>? GetReservationSlots(int interval = 30)
    {
        List<ReservationSlot> slots = new();

        for (int i = 0; i < 24; i++)
        {
            string strAMPM = i < 12 ? "오전" : "오후";
            string strHH = (i % 12).ToString().PadLeft(2, '0');

            for (int j = 0; j < 60; j += interval)
            {
                string strMM = j.ToString().PadLeft(2, '0');
                string actualValue = i.ToString().PadLeft(2, '0') + ":" + strMM;
                string displayValue = strAMPM + " " + actualValue;

                slots.Add(new ReservationSlot { RES_Time = actualValue, vRES_Time = displayValue});
            }
        }

        return slots;
    }

    public async Task<bool> ExisitsReception(int PAT_Idx, string RCP_YYMMDD)
    {
        bool isExisits = false;

        var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, new Reception { PAT_Idx = PAT_Idx, RCP_YYMMDD = RCP_YYMMDD });
        if (retRCP != null)
        {
            isExisits = true;
        }

        return isExisits;
    }
}

public class PAT_ImageSourceToImageConverter : BaseConverter
{
    private static readonly BitmapImage DefaultImage = GlyphImage("Images/smartemr_patient_default_image.png");

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not byte[] bytes || bytes.Length == 0)
        {
            return DefaultImage;
        }

        try
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // 메모리 누수 방지 (중요!)
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze(); // UI 스레드 간 성능 최적화 및 크로스 스레드 예외 방지
                return image;
            }
        }
        catch
        {
            return DefaultImage;
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IntToBooleanConverter : BaseConverter
{
    public bool invert { get; set; } = false;

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return false;

        if (!Int32.TryParse(value.ToString(), out var intValue)) return false;

        var bFlag = intValue == 0 ? true: false;

        return invert ? !bFlag : bFlag;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IntoToContentConveter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || Int32.TryParse(value.ToString(), out var intValue) == false) return "";

        return intValue == 0 ? "등록" : "수정";
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class YNToBooleanConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var strValue = value?.ToString();
        if (string.IsNullOrWhiteSpace(strValue)) return default!;

        return strValue == "y" ? true : false;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var bFlag = (bool)value;

        return bFlag ? "y" : "n";
    }
}