using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MalWhereDomainUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerDomain;
    [SerializeField] private TextMeshProUGUI _owner;
    [SerializeField] private TextMeshProUGUI _document;
    [SerializeField] private TextMeshProUGUI _responsable;
    [SerializeField] private TextMeshProUGUI _country;
    [SerializeField] private TextMeshProUGUI _responsableContact;
    [SerializeField] private TextMeshProUGUI _responsableTechnical;
    [SerializeField] private TextMeshProUGUI _serverOne;
    [SerializeField] private TextMeshProUGUI _serverTwo;
    [SerializeField] private TextMeshProUGUI _createdDate;
    [SerializeField] private TextMeshProUGUI _updateDate;
    [SerializeField] private TextMeshProUGUI _status;

    public void UpdateDomainInfos(Domain infos)
    {
        _headerDomain.SetText(infos.Name);
        _owner.SetText(infos.Owner);
        _document.SetText(infos.Document);
        _responsable.SetText(infos.Responsable);
        _country.SetText(infos.Country);
        _responsableContact.SetText(infos.ResponsableContact);
        _responsableTechnical.SetText(infos.TechnicalContact);
        _serverOne.SetText(infos.ServerOne);
        _serverTwo.SetText(infos.ServerTwo);
        _createdDate.SetText(infos.CreatedDate);
        _updateDate.SetText(infos.UpdatedDate);
        _status.SetText(infos.Status);
    }
}
