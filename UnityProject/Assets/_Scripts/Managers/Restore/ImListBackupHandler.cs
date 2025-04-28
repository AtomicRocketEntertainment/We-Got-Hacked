using System.Collections.Generic;
using UnityEngine;

public class ImListBackupHandler : MonoBehaviour
{
    [SerializeField] private List<SiteBackup> _backups = new List<SiteBackup>();

    public List<SiteBackup> Backups => _backups;
}
