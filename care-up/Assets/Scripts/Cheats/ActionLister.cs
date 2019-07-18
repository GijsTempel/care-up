using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
#if (UNITY_EDITOR)
using UnityEditor.Animations;

#endif
public class ActionLister : MonoBehaviour {
    List<string> actions;
    public Animator currentAnimator;
    StreamWriter writer;
    bool buildActionList = false;

    string[] allActions =
    {
"RightHand.Armature|021_pickUpRight_L_Lib",
"RightHand.Armature|x0040_ipad_closeUp_L_Lib",
"RightHand.Armature|x0050_ipad_far_L_Lib",
"RightHand.Armature|021_pickUpRight_L_Lib 0",
"RightHand.Armature|x0292_prop_test_L_Lib",
"RightHand.Use Animations.Injection Scene.Armature|150_writeOnPaperOnTable_L_Lib",
"RightHand.Use Animations.Injection Scene.Armature|tableCleaning",
"RightHand.Use Animations.Injection Scene.Armature|wash",
"RightHand.Use Animations.MedicineMouth.TakingBagAnimation",
"RightHand.UseOn Animations.Injection Scene.Armature|130_ventTheSyringe_sRight_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|131_ventTheSyringe_sLeft_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|310_removeNeedle_sRight_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|311_removeNeedle_sLeft_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|930_safe_throwNeedle_nLeft_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|931_safe_throwNeedle_nRight_L_Lib",
"RightHand.UseOn Animations.Injection Scene.Armature|840_remove_clothAroundAmpoule_Right_L_Lib",
"RightHand.UseOn Animations.Hameogluco Scene.UseLeft Gloves",
"RightHand.UseOn Animations.Hameogluco Scene.UseRight Gloves",
"RightHand.UseOn Animations.Hameogluco Scene.UseLeft PrickingPen",
"RightHand.UseOn Animations.Hameogluco Scene.UseRight PrickingPen",
"RightHand.UseOn Animations.Hameogluco Scene.UseLeft _",
"RightHand.UseOn Animations.Hameogluco Scene.UseRight _",
"RightHand.UseOn Animations.InsulinInjection.Armature|320_insulinPen_vent_pRight_L_Lib",
"RightHand.UseOn Animations.InsulinInjection.Armature|321_insulinPen_vent_pLeft_L_Lib",
"RightHand.UseOn Animations.InsulinInjection.Armature|350_insulinPen_set10Units_pRight_L_Lib",
"RightHand.UseOn Animations.InsulinInjection.Armature|351_insulinPen_set10Units_pRight_L_Lib",
"RightHand.UseOn Animations.InsulinInjection.Armature|x0020_throwing_needle_nRight_L_Lib",
"RightHand.UseOn Animations.InsulinInjection.Armature|x0021_throwing_needle_nLeft_L_Lib",
"RightHand.Sequence Animations.InjectionSequence_sRight",
"RightHand.Sequence Animations.TutorialInjectionSequence_sRight",
"RightHand.Sequence Animations.Injection v2Sequence_sRight",
"RightHand.Sequence Animations.MeasureBloodGlucose(Haemogluco)Sequence",
"RightHand.Sequence Animations.SubcutaneousInjection",
"RightHand.Sequence Animations.SubcutaneousInjection v2",
"RightHand.Sequence Animations.Armature|1030_insulineInjection_L_Lib_003",
"RightHand.Sequence Animations.Fraxiparine Sequence",
"RightHand.Sequence Animations.Armature|x0230_cleaning_genitals_sq_L_Lib",
"RightHand.Sequence Animations.430",
"RightHand.Sequence Animations.430sq",
"RightHand.Combine Animations.Injection Scene.Armature|070_soakingCloth_aRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|071_soakingCloth_aLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|060_desinfMed_bRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|061_desinfMed_bLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|110_removingNeedle_nLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|111_removingNeedle_nRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|120_absorpMoedicine_mLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|121_absorpMoedicine_mRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|200_removingNeedleCap_sRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|201_removingNeedleCap_sLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|210_placingNeedleCap_nRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|211_placingNeedleCap_nLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0000_desinfAmpoule_aRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0001_desinfAmpoule_aLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0070_syringePack_Open_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|430_safe_placingNeedle_nLeft_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|431_safe_placingNeedle_nRight_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0101_wSyPack_removingCover_sL_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0100_wSyPack_removingCover_sR_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0111_wSy_takeFromPack_sR_L_Lib",
"RightHand.Combine Animations.Injection Scene.Armature|x0110_wSy_takeFromPack_sL_L_Lib",
"RightHand.Combine Animations.Hameogluco Scene.Combine PrickingPen Lancet",
"RightHand.Combine Animations.Hameogluco Scene.Combine Lancet PrickingPen",
"RightHand.Combine Animations.Hameogluco Scene.Combine TestStripCasing _",
"RightHand.Combine Animations.Hameogluco Scene.Combine _ TestStripCasing",
"RightHand.Combine Animations.Hameogluco Scene.Combine TestStripCasing TestStrips",
"RightHand.Combine Animations.Hameogluco Scene.Combine TestStrips TestStripCasing",
"RightHand.Combine Animations.Hameogluco Scene.Combine PrickingPen _",
"RightHand.Combine Animations.Hameogluco Scene.Combine _ PrickingPen",
"RightHand.Combine Animations.InjectionSubcutaneous.Cloth Ampoule",
"RightHand.Combine Animations.InjectionSubcutaneous.Ampoule Cloth",
"RightHand.Combine Animations.InjectionSubcutaneous.OpenedAmpoule SyringeWithAbsorptionNeedle",
"RightHand.Combine Animations.InjectionSubcutaneous.SyringeWithAbsorptionNeedle OpenedAmpoule",
"RightHand.Combine Animations.InjectionSubcutaneous.SyringeWithSolvent Medicine",
"RightHand.Combine Animations.InjectionSubcutaneous.Medicine SyringeWithSolvent",
"RightHand.Combine Animations.SubcutaneousV2.Armature|240_placingSubNeedleCap_nRight_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|241_placingSubNeedleCap_nLeft_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|250_placingSubNeedle_nLeft_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|251_placingSubNeedle_nRight_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|260_removingSubNeedleCap_sRight_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|261_removingSubNeedleCap_sLeft_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|270_removingSubNeedle_nLeft_L_Lib",
"RightHand.Combine Animations.SubcutaneousV2.Armature|271_removingSubNeedle_nRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|331_insulinPen_placing protectionCap_Left_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|330_insulinPen_placing protectionCap_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|190_insulinPen_RemovingCap_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|191_insulinPen_RemovingCap_pLeft_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|220_insulinWrapperRemoving_Right_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|221_insulinWrapperRemoving_Left_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|230_insulinPen_placingNeedle_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|231_insulinPen_placingNeedle_pLeft_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|280_insulinPen_remOuterNCap_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|281_insulinPen_remOuterNCap_pLeft_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|340_insulinPen_removingNeedle_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|311_removeNeedle_sLeft_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|360_insulinPen_swerve_pRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|361_insulinPen_swerve_pLeft_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|x0020_throwing_needle_nRight_L_Lib",
"RightHand.Combine Animations.InsulinInjection.Armature|x0021_throwing_needle_nLeft_L_Lib",
"RightHand.Combine Animations.Fraxi.990",
"RightHand.Combine Animations.Fraxi.991",
"RightHand.Combine Animations.Fraxi.470",
"RightHand.Combine Animations.Fraxi.471",
"RightHand.Combine Animations.Fraxi.Armature|451_fraxi_slideProtection_sLeft_L_Lib",
"RightHand.Combine Animations.Fraxi.Armature|450_fraxi_slideProtection_sRight_L_Lib",
"RightHand.Combine Animations.Fraxi.Armature|460_fraxi_throwing_sRight_L_Lib",
"RightHand.Combine Animations.Fraxi.Armature|461_fraxi_throwing_sLeft_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0131_cloth_02_unfold_cL_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0130_cloth_02_unfold_cR_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0190_wet_gauzeTray_tL_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0191_wet_gauzeTray_tR_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0250_open_catheter_bag_L_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0251_open_catheter_bag_R_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0271_catheter_openOuter_L_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0270_catheter_openOuter_R_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0291_cinematic_test_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0291_cinematic_test_L_Lib 0",
"RightHand.Combine Animations.Catheterisation.Armature|x0311_catheter_inner_openTop_L_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0310_catheter_inner_openTop_R_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0101_wSyPack_removingCover_sL_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0100_wSyPack_removingCover_sR_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0320_catheter_bag_bR_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0321_catheter_bag_bL_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0360_insert_catheter_sq_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0381_fixation_set_L_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0380_fixation_set_R_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0391_fixationButtons_set_L_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0390_fixationButtons_set_R_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0400_help_to_sit_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0420_look_down_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0420_look_down_L_Lib 0",
"RightHand.Combine Animations.Catheterisation.Armature|x0371_open_eyes_L_Lib 0",
"RightHand.Combine Animations.Catheterisation.Armature|x0440_wet_hands_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0450_wet_Alcohol_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0430_cleaning_hands_SQ_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0470_dryHandsWithTowel_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0442_wet_hands_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0480_closeTap_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0481_throwPaperTowel_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0483_holdPaperTowel_L_Lib",
"RightHand.Combine Animations.Catheterisation.Armature|x0131_cloth_02_unfold_cL_L_Lib 0",
"RightHand.Combine Animations.Catheterisation.x0501_cleaning_male_genitals_sq",
"RightHand.Combine Animations.Catheterisation.Armature|x0510_M_insert_catheter_sq_L_Lib_001",
"RightHand.Combine Animations.Catheterisation.521",
"RightHand.Combine Animations.Catheterisation.520",
"RightHand.Combine Animations.Catheterisation.530_help_to_sit",
"RightHand.Combine Animations.Catheterisation.Armature|x0541_fixationButtons_set_L_L_Lib_001",
"RightHand.Combine Animations.Catheterisation.Armature|x0540_fixationButtons_set_R_L_Lib_001",
"RightHand.Combine Animations.Catheterisation.x0481",
"RightHand.Combine Animations.Catheterisation.Armature|x0131_cloth_02_unfold_cR",
"RightHand.Combine Animations.Medicine_mouth.Armature|OpeningBagAnimation_L_L_Lib_001",
"RightHand.Combine Animations.Medicine_mouth.Armature|OpeningBagAnimation_R_L_Lib_001",
"RightHand.Idle-Hold Animations.Armature|010_Idle_L_Lib 0",
"RightHand.Idle-Hold Animations.Armature|x0290_T_test_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|650_medicine_L_Lib 0",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|640_syringe_L_Lib 0",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|630_needle_L_Lib 0",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|620_disposal_L_Lib 0",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|610_alcohol_L_Lib 0",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|660_cloth_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|780_safeNeedleCap_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|670_papper_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|820_papper_close_up_move_Right_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|851_medicine_closeup_Right_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|960_froxi_pack_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|970_fraxi_closeup_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|980_fraxi_far_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|790_froxi_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|x0060_syringePack_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|x0140_wSyPack_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|x0150_wSyringe_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|x0160_wSyPack_no_cover_L_Lib",
"RightHand.Idle-Hold Animations.InjectionScene.Armature|x0471_H_paperTowel_L_Lib",
"RightHand.Idle-Hold Animations.SubcutaneousInjection.Ampoule",
"RightHand.Idle-Hold Animations.SubcutaneousInjection.Folded Cloth",
"RightHand.Idle-Hold Animations.SubcutaneousInjection.Armature|880_ampoule_closeup_L_Lib",
"RightHand.Idle-Hold Animations.SubcutaneousV2.Armature|750_subcutaneousNeedle_L_Lib",
"RightHand.Idle-Hold Animations.InsulinInjection.Armature|710_insulinPen_L_Lib",
"RightHand.Idle-Hold Animations.InsulinInjection.Armature|720_insulinPenCap_L_Lib",
"RightHand.Idle-Hold Animations.InsulinInjection.Armature|730_insulinNeedlePack_L_Lib",
"RightHand.Idle-Hold Animations.InsulinInjection.Armature|760_insulinNeedle_L_Lib",
"RightHand.Idle-Hold Animations.InsulinInjection.Armature|860_insulinPen_closeup_Right_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0120_cloth_02",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0180_gauzeTray_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0200_PlasticTrashbucket_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0210_cotton_ball_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0240_catheter_bag_packed_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0260_catheter_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0280_catheter_bag_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0300_catheter_inner_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0340_catheter_with_bag_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0330_catheter_bag_twisted_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0341_catheter_with_bag_L_Lib",
"RightHand.Idle-Hold Animations.Catheterisation.Armature|x0380_fixation_L_Lib",
"RightHand.Idle-Hold Animations.Medicine_mouth.MedicineBagHold",
"RightHand.Idle-Hold Animations.Medicine_mouth.HoldPill",
"RightHand.Idle-Hold Animations.Medicine_mouth.Armature|4001_ExamineMedicineBag_R_L_Lib_001",

 };

    void Start () {
#if (UNITY_EDITOR)
        if(GameObject.FindObjectOfType<ObjectsIDsController>() != null)
            buildActionList = GameObject.FindObjectOfType<ObjectsIDsController>().buildActionList;
        actions = new List<string>();
        if (GetComponent<Animator>() != null)
        {
            currentAnimator = GetComponent<Animator>();
        }
        else
        {
            currentAnimator = GetComponentInChildren<Animator>();
        }
        string scneName = SceneManager.GetActiveScene().name;
        string path = "Assets/ListOfActions/" + scneName + ".txt";

        if (buildActionList)
        {
            writer = new StreamWriter(path, false);
        }
#endif
    }


    void Update()
    {
#if (UNITY_EDITOR)
        if (currentAnimator != null && writer != null)
        {
            for (int i = 0; i <= currentAnimator.layerCount - 1; i++)
            {
                //print(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name + " | " +
                //    aa[aa.Length - 1]);
                //    AssetDatabase.GetAssetPath(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.GetInstanceID()).Split("/"));

                if (!actions.Contains(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name))
                {
                    string actionPath = "___ ";
                    //foreach (string a in allActions)
                    //{
                    //    if (Animator.StringToHash(a) == currentAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash)
                    //    {
                    //        print("______" + a);
                    //        actionPath = a;
                    //    }
                    //}
                    actions.Add(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name);
                    string[] aa = AssetDatabase.GetAssetPath(currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.GetInstanceID()).Split('/');
                    print("__ " + aa[aa.Length - 1] + " __ " + actionPath);
                    writer.WriteLine(aa[aa.Length - 1] + " __ " + currentAnimator.GetCurrentAnimatorClipInfo(i)[0].clip.name + "\n  ==>  " + actionPath + "\n");

                }
            }
        }
#endif
    }

    void OnApplicationQuit()
    {
#if (UNITY_EDITOR)
        Debug.Log("Application ending after " + Time.time + " seconds");
        if (writer != null)
            writer.Close();
#endif
    }
}
