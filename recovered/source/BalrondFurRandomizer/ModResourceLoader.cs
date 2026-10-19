using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BalrondFurRandomizer;

public class ModResourceLoader
{
	public AssetBundle assetBundle;

	public Material loxFace_Default;

	public Material loxtBody_Default;

	public Material loxFur_Default;

	public Material loxFace_Albino;

	public Material loxtBody_Albino;

	public Material loxFur_Albino;

	public Material loxFace_Black;

	public Material loxtBody_Black;

	public Material loxFur_Black;

	public Material loxFace_Blond;

	public Material loxtBody_Blond;

	public Material loxFur_Blond;

	public Material loxFace_DarkBrown;

	public Material loxtBody_DarkBrown;

	public Material loxFur_DarkBrown;

	public Material loxFace_Ginger;

	public Material loxtBody_Ginger;

	public Material loxFur_Ginger;

	public Material wolfBody_Default;

	public Material wolfBody_Albino;

	public Material wolfBody_Black;

	public Material wolfBody_DarkBrown;

	public Material wolfBody_Grey;

	public Material boarBody_Default;

	public Material boarBody_Grey;

	public Material boarBody_Black;

	public Material boarBody_DarkBrown;

	public Material boarBody_LightBrown;

	public Material askvinBody_Default;

	public Material askvinBody_Albino;

	public Material askvinBody_Purple;

	public Material askvinBody_Black;

	public Material askvinBody_Grey;

	public Material deerBody_Default;

	public Material deerBody_Albino;

	public Material deerBody_Red;

	public Material deerBody_DarkBrown;

	public Material deerBody_Grey;

	public Material neckBody_Default;

	public Material neckBody_Albino;

	public Material neckBody_Blue;

	public Material neckBody_DarkBlue;

	public Material neckBody_DarkGreen;

	public Material hareBody_Default;

	public Material hareBody_Albino;

	public Material hareBody_Black;

	public Material hareBody_DarkBlue;

	public Material hareBody_Ginger;

	public Material drakeBody_Default;

	public Material drakeBody_DarkBlue;

	public Material drakeBody_IceBlue;

	public Material drakeBody_Purple;

	public Material drakeBody_Red;

	public Material serpentBody_Default;

	public Material serpentBody_Albino;

	public Material serpentBody_Black;

	public Material serpentBody_Blue;

	public Material serpentBody_Green;

	public Material serpentBody_Red;

	public void loadAssets()
	{
		assetBundle = GetAssetBundleFromResources("furrandomizer");
		string text = "Assets/BalrondFurRandomizer/";
		Material temp = assetBundle.LoadAsset<Material>(text + "Lox/Default/lox_face.mat");
		CreateLoxMaterial(temp, text);
		CreateWolfMaterial(temp, text);
		CreateNeckMaterial(temp, text);
		CreateHareMaterial(temp, text);
		CreateDrakeMaterial(temp, text);
		CreateSerpentMaterials(temp, text);
		CreateAskvinMaterial(temp, text);
		CreateBoarMaterial(temp, text);
		CreateDeerMaterial(temp, text);
	}

	private void CreateLoxMaterial(Material temp, string mainPath)
	{
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Default/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Default/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Albino/lox_face.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Albino/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Albino/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Black/lox_face.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Black/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Black/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Blonde/lox_face.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_Blond = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Blonde/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_Blond = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Blonde/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_Blond = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/DarkBrown/lox_face.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/DarkBrown/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/DarkBrown/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Ginger/lox_face.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFace_Ginger = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Ginger/lox_body1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxtBody_Ginger = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Lox/Ginger/lox_fur1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		loxFur_Ginger = temp;
	}

	private void CreateWolfMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Wolf/Default/wolfMat.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		wolfBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Wolf/Albino/wolfMat.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		wolfBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Wolf/Black/wolfMat.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		wolfBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Wolf/DarkBrown/wolfMat.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		wolfBody_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Wolf/Grey/wolfMat.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		wolfBody_Grey = temp;
	}

	private void CreateBoarMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Boar/Default/Material_default.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		boarBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Boar/Grey/Material_grey.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		boarBody_Grey = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Boar/Black/Material_black.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		boarBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Boar/DarkBrown/Material_darkBrown.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		boarBody_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Boar/Young/Material_young.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		boarBody_LightBrown = temp;
	}

	private void CreateDeerMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Deer/Default/Deer_default.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		deerBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Deer/Albino/Deer_albino.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		deerBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Deer/Red/Deer_red.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		deerBody_Red = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Deer/LightBrown/Deer_brown.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		deerBody_DarkBrown = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Deer/Grey/Deer_gray.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		deerBody_Grey = temp;
	}

	private void CreateAskvinMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Askvin/Default/asksvin_Mat_default.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		askvinBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Askvin/Albino/asksvin_Mat_albino.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		askvinBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Askvin/Purple/asksvin_Mat_purple.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		askvinBody_Purple = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Askvin/Black/asksvin_Mat_black.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		askvinBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Askvin/Young/asksvincub_Mat_young.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		askvinBody_Grey = temp;
	}

	private void CreateNeckMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Neck/Default/neck1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		neckBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Neck/Albino/neck1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		neckBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Neck/Blue/neck1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		neckBody_Blue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Neck/DarkBlue/neck1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		neckBody_DarkBlue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Neck/DarkGreen/neck1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		neckBody_DarkGreen = temp;
	}

	private void CreateHareMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Hare/Default/Hare_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		hareBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Hare/Albino/Hare_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		hareBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Hare/Black/Hare_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		hareBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Hare/DarkBlue/Hare_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		hareBody_DarkBlue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Hare/Ginger/Hare_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		hareBody_Ginger = temp;
	}

	private void CreateDrakeMaterial(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Drake/Default/Hatchling_mountain_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		drakeBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Drake/DarkBlue/Hatchling_mountain_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		drakeBody_DarkBlue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Drake/IceBlue/Hatchling_mountain_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		drakeBody_IceBlue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Drake/Purple/Hatchling_mountain_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		drakeBody_Purple = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Drake/Red/Hatchling_mountain_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		drakeBody_Red = temp;
	}

	private void CreateSerpentMaterials(Material temp, string mainPath)
	{
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Default/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Default = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Albino/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Albino = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Black/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Black = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Blue/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Blue = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Green/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Green = temp;
		temp = assetBundle.LoadAsset<Material>(mainPath + "Serpent/Red/SeaSerpent_mat1.mat");
		ShaderReplacment.ReplaceMaterial(temp);
		serpentBody_Red = temp;
	}

	private AssetBundle GetAssetBundleFromResources(string filename)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		string name = executingAssembly.GetManifestResourceNames().Single((string str) => str.EndsWith(filename));
		using Stream stream = executingAssembly.GetManifestResourceStream(name);
		return AssetBundle.LoadFromStream(stream);
	}
}
