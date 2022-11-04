using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region Declarations
    
    [Tooltip("La liste des AudioSources qui vont jouer les effets sonores du jeu, à récupérer directement sur l'objet actuel")]
    [SerializeField] public AudioSource[] audioSources;

    public static AudioManager instance;
    [HideInInspector] public int audioSourceLenght = 0;
    [HideInInspector] public AudioSource[] audioSourcesEditor;

    private Tools tools;

    #endregion
    
    private void Awake()
    {
        instance = this;
        tools = new Tools();
    }

    // Joue le son
    public void PlaySound(AudioClip sound, float volume, float speed, float time)
    {

        #region Audiosource free
        
        // Cherche une place de libre dans l'array
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] == null || !audioSources[i].isPlaying)
            {
                audioSources[i].clip = sound;
                audioSources[i].volume = volume;
                audioSources[i].time = time;
                audioSources[i].pitch = speed;
                audioSources[i].Play();
                return;
            }
        }
        
        #endregion

        #region Audiosource not free
        
        //Si pas de place remplace le son le plus ancien
        
        tools.Sort(audioSources);
        audioSources[audioSources.Length-1].clip = sound;
        audioSources[audioSources.Length-1].volume = volume;
        audioSources[audioSources.Length-1].time = time;
        audioSources[audioSources.Length-1].pitch = speed;
        
        #endregion
    }



    // Arrête tous les sons
    public void StopSounds()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
            {
                audioSources[i].Stop();
            }
        }
    }
}
