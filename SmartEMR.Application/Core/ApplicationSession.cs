using SmartEMR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEMR.Application.Core;

public class ApplicationSession
{
    private static ApplicationSession? _instance;
    public static ApplicationSession Instance => _instance ?? (_instance = new ApplicationSession());

    public DataStore DataStore { get; } = new DataStore();

    public readonly string APIUrl = "127.0.0.1";

    public int? MUR_Idx { get; set; }

    private ApplicationSession() { }

    public void Initialize()
    {
        // API URL 설정
        DataStore.APIUrl = $"http://{APIUrl}:8000";
    }
}