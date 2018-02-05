using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InjectionPatient : PersonObject {

    private AudioClip[] audioClips;

    public GameObject greetDialogueTrigger;
    
    private bool greetDialogueTriggered = false;

    protected override void Start()
    {
        base.Start();
        
        audioClips = new AudioClip[17];

        for (int i = 0; i < 17; ++i)
        {
            string name = "Audio/Injection/Dialog/" + (i + 1);
            audioClips[i] = Resources.Load<AudioClip>(name);
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (Vector3.Distance(transform.position, greetDialogueTrigger.transform.position) >
            Vector3.Distance(transform.position, player.transform.position))
        {
            GreetDialogue();
        }
    }

    public void GreetDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;
    
        if (greetDialogueTriggered)
            return;
        else
        {
            greetDialogueTriggered = true;
            StartCoroutine(GreetDialogueCoroutine());
        }
    }

    private IEnumerator GreetDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[0]);
        yield return new WaitForSeconds(audioClips[0].length);
        audioSource.PlayOneShot(audioClips[1]);
        yield return new WaitForSeconds(audioClips[1].length);
        Narrator.PlaySound(audioClips[2]);
        yield return new WaitForSeconds(audioClips[2].length);
        audioSource.PlayOneShot(audioClips[3]);
        yield return new WaitForSeconds(audioClips[3].length);
        Narrator.PlaySound(audioClips[4]);
        yield return new WaitForSeconds(audioClips[4].length);
        audioSource.PlayOneShot(audioClips[5]);
    }

    public void PutAbsorptionNeedleDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;

        StartCoroutine(PutAbsorptionNeedleDialogueCoroutine());
    }

    private IEnumerator PutAbsorptionNeedleDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[6]);
        yield return new WaitForSeconds(audioClips[6].length);
        audioSource.PlayOneShot(audioClips[7]);
        yield return new WaitForSeconds(audioClips[7].length);
        Narrator.PlaySound(audioClips[8]);
    }

    public void RollUpSleevesDialogue()
    {
        StartCoroutine(RollUpSleevesDialogueCoroutine());
    }

    private IEnumerator RollUpSleevesDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[9]);
        yield return new WaitForSeconds(audioClips[9].length);
        audioSource.PlayOneShot(audioClips[10]);
        yield return new WaitForSeconds(audioClips[10].length);
        GetComponent<Animator>().SetTrigger("ShowArm");
    }

    public void InjectNeedleInArmDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;

        StartCoroutine(InjectNeedleInArmDialogueCoroutine());
    }

    private IEnumerator InjectNeedleInArmDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[11]);
        yield return new WaitForSeconds(audioClips[11].length);
        audioSource.PlayOneShot(audioClips[12]);
        yield return new WaitForSeconds(audioClips[12].length);
        PlayerAnimationManager.NextSequenceStep(false);
    }

    public void InjectMedicineSlowlyDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;

        StartCoroutine(InjectMedicineSlowlyDialogueCoroutine());
    }

    private IEnumerator InjectMedicineSlowlyDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[13]);
        yield return new WaitForSeconds(audioClips[13].length);
        audioSource.PlayOneShot(audioClips[14]);
    }

    public void AfterSequenceDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;

        StartCoroutine(AfterSequenceDialogueCoroutine());
    }

    private IEnumerator AfterSequenceDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[15]);
        yield return new WaitForSeconds(audioClips[15].length);
        audioSource.PlayOneShot(audioClips[16]);
    }
}
