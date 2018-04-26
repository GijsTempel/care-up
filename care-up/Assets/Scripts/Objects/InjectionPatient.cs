using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InjectionPatient : PersonObject {

    private AudioClip[] audioClips;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        
        audioClips = new AudioClip[17];

        for (int i = 0; i < 17; ++i)
        {
            string name = "Audio/Injection/Dialog/" + (i + 1);
            audioClips[i] = Resources.Load<AudioClip>(name);
        }

        animator = GetComponent<Animator>();
    }

    public override void Talk(string topic = "")
    {
        if (ViewModeActive() || topic == "")
            return;

        if (actionManager.CompareTopic(topic))
        {
            tutorial_talked = true;
            actionManager.OnTalkAction(topic);

            switch (topic)
            {
                case "Hello":
                    GreetDialogue();
                    break;
                case "RollUpSleeves":
                case "ExtendArmMakeFist":
                    if (GetComponent<InjectionPatient>() != null)
                    {
                        // also launches animation after dialogue
                        GetComponent<InjectionPatient>().RollUpSleevesDialogue();
                    }
                    break;
                case "ComfortablePosition":
                    inhaling = true;
                    break;
                case "ShowBellyForInsulin":
                    GetComponent<Animator>().SetTrigger("ShowBellyForInsulin");
                    break;
                default:
                    break;
            }

            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        ++currentDialogueIndex;
        if (currentDialogueIndex < dialogueXmls.Count)
        {
            LoadDialogueOptions(dialogueXmls[currentDialogueIndex]);
        }
    }

    public void GreetDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
            return;

        StartCoroutine(GreetDialogueCoroutine());
    }

    public void GreetDialoguePt2()
    {
        StartCoroutine(GreetDialogueCoroutinePt2());
    }

    public void GreetDialoguePt3()
    {
        StartCoroutine(GreetDialogueCoroutinePt3());
    }

    private IEnumerator GreetDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[0]);
        yield return new WaitForSeconds(audioClips[0].length);

        animator.SetTrigger("goedemorgen");
        audioSource.PlayOneShot(audioClips[1]);
        yield return new WaitForSeconds(audioClips[1].length);
    }

    private IEnumerator GreetDialogueCoroutinePt2()
    {
        Narrator.PlaySound(audioClips[2]);
        yield return new WaitForSeconds(audioClips[2].length);

        animator.SetTrigger("ja_is_goed");
        audioSource.PlayOneShot(audioClips[3]);
        yield return new WaitForSeconds(audioClips[3].length);
    }

    private IEnumerator GreetDialogueCoroutinePt3()
    {
        Narrator.PlaySound(audioClips[4]);
        yield return new WaitForSeconds(audioClips[4].length);

        animator.SetTrigger("oke");
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
        animator.SetTrigger("ja_goed_hoor");
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
        animator.SetTrigger("ja_hoor_zal_ik_doen");
        audioSource.PlayOneShot(audioClips[10]);
        yield return new WaitForSeconds(audioClips[10].length);
        animator.SetTrigger("ShowArm");
        lookAtCamera = false;
    }

    public void InjectNeedleInArmDialogue()
    {
        StartCoroutine(InjectNeedleInArmDialogueCoroutine());
    }

    private IEnumerator InjectNeedleInArmDialogueCoroutine()
    {
        Narrator.PlaySound(audioClips[11]);
        yield return new WaitForSeconds(audioClips[11].length);
        animator.SetTrigger("oke");
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
        animator.SetTrigger("ja_hoor");
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
        animator.SetTrigger("oke_dat_wil_mee");
        audioSource.PlayOneShot(audioClips[16]);
    }
}
