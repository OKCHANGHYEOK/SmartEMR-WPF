using SmartEMR.Application.Common;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class ReservationService : BaseService, IReservationService
{
    public ReservationService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<Reservation>> GetReservation(Reservation item)
    {
        var result = new ServiceResult<Reservation>();
        var ret = await _dataStore.GetItem<Reservation>(eAPI.Reservation_GetReservation, item);
        
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "예약 데이터를 불러오는데 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reservation>> GetReservations(Reservation item)
    {
        var result = new ServiceResult<Reservation>();
        var ret = await _dataStore.GetItems<Reservation>(eAPI.Reservation_GetReservation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "예약내역을 불러오지 못했습니다.";
            return result;
        }

        DisplayDataMappers.ReservationDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reservation>> SetReservation(Reservation item)
    {
        var result = new ServiceResult<Reservation>();
        var ret = await _dataStore.GetItem<Reservation>(eAPI.Reservation_SetReservation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = _dataStore.retMessage;
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reservation>> SetReservationByStatus(Reservation item)
    {
        var result = new ServiceResult<Reservation>();
        var ret = await _dataStore.GetItem<Reservation>(eAPI.Reservation_SetReservationByStatus, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "예약상태를 업데이트하는데 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reservation>> MoveReservationDate(Reservation item)
    {
        var result = new ServiceResult<Reservation>();
        var ret = await _dataStore.GetItem<Reservation>(eAPI.Reservation_MoveReservationDate, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = $"예약일시 변경에 실패했습니다.\n{_dataStore.retMessage}";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
