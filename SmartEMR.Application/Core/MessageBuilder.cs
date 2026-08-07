using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Core;

public class MessageBuilder
{
    public static List<Inline> CreateReservationInfo(Reservation item)
    {
        var messages = new List<Inline>();

        AddLabelValue(messages, "예약자명 : ", item.PAT_Name ?? "", false);

        messages.Add(new Run("(") { Foreground = Brushes.DimGray });
        messages.Add(new Run(item.PAT_Sex == "M" ? "남" : "여") { Foreground = Brushes.DimGray });
        messages.Add(new Run("/") { Foreground = Brushes.DimGray });
        messages.Add(new Run($"{item.PAT_Age}세") { Foreground = Brushes.DimGray });
        messages.Add(new Run(")") { Foreground = Brushes.DimGray });
        messages.Add(new LineBreak());

        AddLabelValue(messages, "예약일시 : ", $"{item.RES_ReservationDate} {item.RES_ReservationTime}");

        string? MUR_Name_DOC = "미정";

        if (item.MUR_Idx_DOC > 0)
        {
            MUR_Name_DOC = SmartMVVM.Master.GetMemberUsers("DOC").FirstOrDefault(x => x.MUR_Idx == item.MUR_Idx_DOC)?.MUR_Name;
        }

        AddLabelValue(messages, "담당의 : ", MUR_Name_DOC ?? "");

        messages.Add(CreateLabel("예약과목 : "));
        messages.Add(new Run(" "));

        if (item.RES_Subject == "ETC")
        {
            messages.Add(new Run("기타진료"));
            messages.Add(new Run($" ({item.RES_SubjectName})"));
        }
        else
        {
            messages.Add(new Run(SmartMVVM.Common.GetCommonCodeName("RES", "Subject", item.RES_Subject ?? "")));
        }

        messages.Add(new LineBreak());

        AddLabelValue(messages, "예약메모 : ", item.RES_Memo ?? "");

        return messages;
    }

    private static InlineUIContainer CreateLabel(string text)
    {
        return new InlineUIContainer(new TextBlock { Text = text, Width = 70, VerticalAlignment = VerticalAlignment.Center });
    }

    private static void AddLabelValue(List<Inline> messages, string label, string value, bool isLineBreak = true)
    {
        messages.Add(CreateLabel(label));
        messages.Add(new Run(" "));
        messages.Add(new Run(value));

        if (isLineBreak)
        {
            messages.Add(new LineBreak());
        }
    }
}
