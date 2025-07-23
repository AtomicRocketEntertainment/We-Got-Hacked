using UnityEngine;
using TMPro;

public class TicketInfoPopUpScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playbook;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _ipOrigin;
    [SerializeField] private TextMeshProUGUI _ipDestiny;
    [SerializeField] private TextMeshProUGUI _localization;
    [SerializeField] private TextMeshProUGUI _dispositive;
    [SerializeField] private TextMeshProUGUI _date;
    [SerializeField] private TextMeshProUGUI _risk;
    [SerializeField] private TextMeshProUGUI _site;

    public void UpdateInfo(Ticket ticket)
    {
        string webSite = ticket.Site == SiteType.Others ? "Não se aplica." : "sustentabilidade.petrocais.com.br";
        _playbook.text = $"Playbook: {ticket.Playbook}";
        _id.text = $"ID: {ticket.ID}";
        _ipOrigin.text = $"IP Origem: {ticket.IPOrigem}";
        _ipDestiny.text = $"IP Destino: {ticket.IPDestiny}";
        _localization.text = $"Geolocalização: {ticket.Location}";
        _dispositive.text = $"Dispositivo: {ticket.Dispositive}";
        _date.text = $"Data: {ticket.DateDay} - {ticket.DateHour}";
        _risk.text = $"Risco: {ticket.RiskLevel}";
        _site.text = $"Website: {webSite}";
    }

}