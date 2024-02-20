using UnityEngine;    
using System.Collections;    
using UnityEngine.UI;
using UnityEngine.EventSystems;
    
[RequireComponent (typeof (AudioSource))]    
public class RecordAndPlayback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler      
{    
    // Boolean flags shows if the microphone is connected   
    private bool micConnected = false;    
    
    //The maximum and minimum available recording frequencies    
    private int minFreq;    
    private int maxFreq; 
    float timer;
    [SerializeField] public Button Button;
    [SerializeField] public Sprite RegularButton;
    [SerializeField] public Sprite HeldButton;   
    
    //A handle to the attached AudioSource    
    private AudioSource goAudioSource;
        
     
    void Start()     
    {    
        //Check if there is at least one microphone connected    
        if(Microphone.devices.Length <= 0)    
        {    
            //Throw a warning message at the console if there isn't    
            Debug.LogWarning("Microphone not connected!");    
        }    
        else //At least one microphone is present    
        {    
            //Set our flag 'micConnected' to true    
            micConnected = true;    
    
            //Get the default microphone recording capabilities    
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);    
    
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...    
            if(minFreq == 0 && maxFreq == 0)    
            {    
                //...meaning 44100 Hz can be used as the recording sampling rate    
                maxFreq = 44100;    
            }    
    
            //Get the attached AudioSource component    
            goAudioSource = this.GetComponent<AudioSource>();    
        }    
    }

    void Update(){
    if (!buttonPressed)
        {
            Microphone.End(null); //Stop the audio recording 
            Debug.Log(timer);
            timer = 0.0f; // Resets the buttonPressed timer so the onclick function works if the button's not held in again
            Button.image.sprite = RegularButton;
            return;
        }
      
        if(buttonPressed){
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                if(micConnected)    
                {    
                    //If the audio from any microphone isn't being captured    
                    if(!Microphone.IsRecording(null))    
                    {  
                        Debug.Log("recording.");
                        Button.image.sprite = HeldButton;
                        goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);    
                    }
                }
            }
            
        }
    }   

    public bool buttonPressed;
    
    public void OnPointerDown(PointerEventData eventData){
        buttonPressed = true;
    }
    
    public void OnPointerUp(PointerEventData eventData){
        buttonPressed = false;
    }

    public void Play(){
            if (timer < 0.2f) {
                goAudioSource.Play(); //Playback the recorded audio  
            }
    }
      
}