using System;
using System.IO;
using System.Text;

namespace Asynchrony
{
    class Program
    {
        public const string LargeTextFilePath = "LargeTextFile.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            GenerateLargeTextFile();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void GenerateLargeTextFile()
        {
            //Generates a 100MB text file.
            if (File.Exists(LargeTextFilePath))
                return;

            var sb = new StringBuilder();
            //Create paragraphs
            for (int i = 0; i < 3300; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    sb.AppendLine(
                        @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam si concederetur, etiamsi ad corpus nihil referatur, ista sua sponte et per se esse iucunda, per se esset et virtus et cognitio rerum, quod minime ille vult expetenda. Sin est etiam corpus, ista explanatio naturae nempe hoc effecerit, ut ea, quae ante explanationem tenebamus, relinquamus. Nam sunt et in animo praecipua quaedam et in corpore, quae cum leviter agnovit, tum discernere incipit, ut ea, quae prima data sunt natura, appetat asperneturque contraria. Nam cum Academicis incerta luctatio est, qui nihil affirmant et quasi desperata cognitione certi id sequi volunt, quodcumque veri simile videatur.

Negarine ullo modo possit numquam quemquam stabili et firmo et magno animo, quem fortem virum dicimus, effici posse, nisi constitutum sit non esse malum dolorem? Leonidas autem, rex Lacedaemoniorum, se in Thermopylis trecentosque eos, quos eduxerat Sparta, cum esset proposita aut fuga turpis aut gloriosa mors, opposuit hostibus. Si scieris, inquit Carneades, aspidem occulte latere uspiam, et velle aliquem inprudentem super eam assidere, cuius mors tibi emolumentum futura sit, improbe feceris, nisi monueris ne assidat, sed inpunite tamen; Haec non erant eius, qui innumerabilis mundos infinitasque regiones, quarum nulla esset ora, nulla extremitas, mente peragravisset. Plane idem, inquit, et maxima quidem, qua fieri nulla maior potest. Hanc se tuus Epicurus omnino ignorare dicit quam aut qualem esse velint qui honestate summum bonum metiantur. Quin etiam ferae, inquit Pacuvius, quíbus abest, ad praécavendum intéllegendi astútia, iniecto terrore mortis horrescunt. Qui enim existimabit posse se miserum esse beatus non erit. Quarum cum una sit, qua mores conformari putantur, differo eam partem, quae quasi stirps ets huius quaestionis.

Huic Epicurus praecentet, si potest, cui e viperino morsu venae viscerum Veneno inbutae taetros cruciatus cient! Sic Epicurus: Philocteta, st! brevis dolor. Quaesita enim virtus est, non quae relinqueret naturam, sed quae tueretur. Ex quo intellegi debet homini id esse in bonis ultimum, secundum naturam vivere, quod ita interpretemur: vivere ex hominis natura undique perfecta et nihil requirente. Atqui si, ut convenire debet inter nos, est quaedam appetitio naturalis ea, quae secundum naturam sunt, appetens, eorum omnium est aliquae summa facienda. Quid autem dici poterit, si turpitudinem non ipsam per se fugiendam esse statuemus, quo minus homines tenebras et solitudinem nacti nullo dedecore se abstineant, nisi eos per se foeditate sua turpitudo ipsa deterreat? Nam ex eisdem verborum praestrigiis et regna nata vobis sunt et imperia et divitiae, et tantae quidem, ut omnia, quae ubique sint, sapientis esse dicatis. An ne hoc quidem Peripateticis concedis, ut dicant omnium bonorum virorum, id est sapientium omnibusque virtutibus ornatorum, vitam omnibus partibus plus habere semper boni quam mali? Fortitudinis quaedam praecepta sunt ac paene leges, quae effeminari virum vetant in dolore. Et tamen quid attinet luxuriosis ullam exceptionem dari aut fingere aliquos, qui, cum luxuriose viverent, a summo philosopho non reprehenderentur eo nomine dumtaxat, cetera caverent? Omnis autem in quaerendo, quae via quadam et ratione habetur, oratio praescribere primum debet ut quibusdam in formulis ea res agetur, ut, inter quos disseritur, conveniat quid sit id, de quo disseratur.

Aequam igitur pronuntiabit sententiam ratio adhibita primum divinarum humanarumque rerum scientia, quae potest appellari rite sapientia, deinde adiunctis virtutibus, quas ratio rerum omnium dominas, tu voluptatum satellites et ministras esse voluisti. Ab his oratores, ab his imperatores ac rerum publicarum principes extiterunt. Quae qui non vident, nihil umquam magnum ac cognitione dignum amaverunt. Duo Reges: constructio interrete. Nam ex eisdem verborum praestrigiis et regna nata vobis sunt et imperia et divitiae, et tantae quidem, ut omnia, quae ubique sint, sapientis esse dicatis. Sed fortuna fortis; Et saepe officium est sapientis desciscere a vita, cum sit beatissimus, si id oportune facere possit, quod est convenienter naturae. Cum autem paulum firmitatis accessit, et animo utuntur et sensibus conitunturque, ut sese erigant, et manibus utuntur et eos agnoscunt, a quibus educantur. Tecum optime, deinde etiam cum mediocri amico. Magno hic ingenio, sed res se tamen sic habet, ut nimis imperiosi philosophi sit vetare meminisse. Cum autem ad summum bonum volunt pervenire, transiliunt omnia et duo nobis opera pro uno relinquunt, ut alia sumamus, alia expetamus, potius quam uno fine utrumque concluderent. Quis, quaeso, inquit, est, qui quid sit voluptas nesciat, aut qui, quo magis id intellegat, definitionem aliquam desideret? Critolaus imitari voluit antiquos, et quidem est gravitate proximus, et redundat oratio, ac tamen is quidem in patriis institutis manet.

Videmus in quodam volucrium genere non nulla indicia pietatis, cognitionem, memoriam, in multis etiam desideria videmus. Quoniamque in iis rebus, quae neque in virtutibus sunt neque in vitiis, est tamen quiddam, quod usui possit esse, tollendum id non est. Ita est quoddam commune officium sapientis et insipientis, ex quo efficitur versari in iis, quae media dicamus. Materiam vero rerum et copiam apud hos exilem, apud illos uberrimam reperiemus. Immo istud quidem, inquam, quo loco quidque, nisi iniquum postulo, arbitratu meo. Legimus tamen Diogenem, Antipatrum, Mnesarchum, Panaetium, multos alios in primisque familiarem nostrum Posidonium. Utrum enim sit voluptas in iis rebus, quas primas secundum naturam esse diximus, necne sit ad id, quod agimus, nihil interest. Obscura, inquit, quaedam esse confiteor, nec tamen ab illis ita dicuntur de industria, sed inest in rebus ipsis obscuritas. Nec enim, cum tua causa cui commodes, beneficium illud habendum est, sed faeneratio, nec gratia deberi videtur ei, qui sua causa commodaverit. Ita fit ut, quanta differentia est in principiis naturalibus, tanta sit in finibus bonorum malorumque dissimilitudo. Quae cum dixissem, magis ut illum provocarem quam ut ipse loquerer, tum Triarius leniter arridens: Tu quidem, inquit, totum Epicurum paene e philosophorum choro sustulisti.");
                }
            }

            File.WriteAllText(LargeTextFilePath, sb.ToString());
        }
    }
}
