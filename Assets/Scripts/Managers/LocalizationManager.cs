using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [SerializeField] private LocalizationSettings m_settings;

    private SystemLanguage m_locale;
    private ReactiveDictionary<string, string> m_localizationTable = new();

    public SystemLanguage Locale
    {
        get => m_locale;
        set
        {
            var oldLocale = m_locale;
            m_locale = value;
            if (oldLocale != m_locale)
            {
                LoadMessages();
            }
        }
    }

    public IReactiveDictionary<string, string> LocalizationTable => m_localizationTable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        m_locale = Application.systemLanguage;
        LoadMessages();
    }

    private void LoadMessages()
    {
        var table = Resources.Load<LocalizationTable>(
            m_settings.localesResourcesPath + m_locale.ToString()
        );
        if (table == null)
        {
            if (m_locale == m_settings.fallbackLocale)
            {
                Debug.LogError($"Localization table for {m_locale} locale do not exist");
                return;
            }
            else
            {
                m_locale = m_settings.fallbackLocale;
                LoadMessages();
                return;
            }
        }

        m_localizationTable.UpdateFrom(Utils.CreateDictionaryFromItems(table.pairs));
    }
}
