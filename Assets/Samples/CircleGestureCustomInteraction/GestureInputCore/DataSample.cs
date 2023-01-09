using UnityEngine;

namespace Samples.SimpleDemo.GestureInputCore
{
    public class DataSample
    {
        public static readonly Vector2[] dataSampleBottomLeftClose = new Vector2[]
        {
            new Vector2(329, 456), new Vector2(334, 444), new Vector2(340, 433), new Vector2(347, 422), 
            new Vector2(361, 405), new Vector2(375, 389), new Vector2(392, 375), new Vector2(401, 369), 
            new Vector2(409, 364), new Vector2(420, 359), new Vector2(430, 354), new Vector2(440, 351), 
            new Vector2(450, 348), new Vector2(471, 343), new Vector2(493, 339), new Vector2(512, 336), 
            new Vector2(532, 334), new Vector2(534, 334), new Vector2(537, 334), new Vector2(548, 332), 
            new Vector2(559, 331), new Vector2(570, 330), new Vector2(581, 329), new Vector2(587, 329), 
            new Vector2(593, 328), new Vector2(601, 327), new Vector2(609, 326), new Vector2(614, 325), 
            new Vector2(618, 325), new Vector2(621, 325), new Vector2(625, 325), new Vector2(631, 324), 
            new Vector2(637, 323)
        };
        
        public static readonly Vector2[] dataSampleBottomLeft = new Vector2[]
        {
            new Vector2(552, 337), new Vector2(552, 337), new Vector2(553, 336), new Vector2(554, 336),
            new Vector2(562, 329), new Vector2(570, 323), new Vector2(573, 321), new Vector2(576, 320),
            new Vector2(586, 315), new Vector2(596, 310), new Vector2(607, 306), new Vector2(617, 301),
            new Vector2(630, 297), new Vector2(643, 293), new Vector2(652, 292), new Vector2(661, 290),
            new Vector2(676, 288), new Vector2(691, 285), new Vector2(709, 284), new Vector2(727, 282),
            new Vector2(742, 281), new Vector2(757, 281), new Vector2(770, 281), new Vector2(783, 281)
        };

        public static readonly Vector2[] dataSampleBottomRight = new Vector2[]
        {
            new Vector2(797, 281), new Vector2(811, 282), new Vector2(826, 285), new Vector2(840, 287),
            new Vector2(852, 291), new Vector2(865, 295), new Vector2(876, 299), new Vector2(887, 304),
            new Vector2(896, 310), new Vector2(906, 317), new Vector2(914, 323), new Vector2(921, 329),
            new Vector2(928, 337), new Vector2(934, 345), new Vector2(939, 354), new Vector2(943, 364),
            new Vector2(947, 374), new Vector2(950, 384), new Vector2(951, 395), new Vector2(953, 406),
            new Vector2(954, 423), new Vector2(954, 439), new Vector2(954, 455), new Vector2(954, 471),
            new Vector2(952, 486), new Vector2(950, 502), new Vector2(936, 539), new Vector2(921, 576),
            new Vector2(911, 592), new Vector2(901, 607), new Vector2(888, 624), new Vector2(876, 640),
            new Vector2(853, 663),
        };

        public static readonly Vector2[] dataSampleTopRight = new Vector2[]
        {
            new Vector2(830, 686), new Vector2(821, 693), new Vector2(811, 700),
            new Vector2(793, 713), new Vector2(775, 727), new Vector2(757, 738), new Vector2(738, 750),
            new Vector2(718, 760), new Vector2(698, 769), new Vector2(683, 775), new Vector2(669, 782), 
            new Vector2(636, 790), new Vector2(603, 799), new Vector2(579, 802), new Vector2(555, 805),
            new Vector2(537, 807), new Vector2(518, 808)
        };

        public static readonly Vector2[] dataSampleTopLeft = new Vector2[]
        {
            new Vector2(486, 806), new Vector2(455, 804),
            new Vector2(442, 799), new Vector2(428, 794), new Vector2(416, 788), new Vector2(403, 782),
            new Vector2(392, 774), new Vector2(381, 766), new Vector2(372, 757), new Vector2(362, 749),
            new Vector2(354, 738), new Vector2(347, 728), new Vector2(340, 716), new Vector2(332, 703),
            new Vector2(326, 688), new Vector2(320, 673), new Vector2(316, 658), new Vector2(312, 643),
            new Vector2(310, 625), new Vector2(307, 607), new Vector2(307, 592), new Vector2(306, 576),
            new Vector2(306, 563), new Vector2(306, 551), new Vector2(307, 537), new Vector2(307, 524)
        };
        public static readonly Vector2[] dataSample = new Vector2[]
        {
            new Vector2(552, 337), new Vector2(552, 337), new Vector2(553, 336), new Vector2(554, 336),
            new Vector2(562, 329), new Vector2(570, 323), new Vector2(573, 321), new Vector2(576, 320),
            new Vector2(586, 315), new Vector2(596, 310), new Vector2(607, 306), new Vector2(617, 301),
            new Vector2(630, 297), new Vector2(643, 293), new Vector2(652, 292), new Vector2(661, 290),
            new Vector2(676, 288), new Vector2(691, 285), new Vector2(709, 284), new Vector2(727, 282),
            new Vector2(742, 281), new Vector2(757, 281), new Vector2(770, 281), new Vector2(783, 281),
            new Vector2(797, 281), new Vector2(811, 282), new Vector2(826, 285), new Vector2(840, 287),
            new Vector2(852, 291), new Vector2(865, 295), new Vector2(876, 299), new Vector2(887, 304),
            new Vector2(896, 310), new Vector2(906, 317), new Vector2(914, 323), new Vector2(921, 329),
            new Vector2(928, 337), new Vector2(934, 345), new Vector2(939, 354), new Vector2(943, 364),
            new Vector2(947, 374), new Vector2(950, 384), new Vector2(951, 395), new Vector2(953, 406),
            new Vector2(954, 423), new Vector2(954, 439), new Vector2(954, 455), new Vector2(954, 471),
            new Vector2(952, 486), new Vector2(950, 502), new Vector2(936, 539), new Vector2(921, 576), 
            new Vector2(911, 592), new Vector2(901, 607), new Vector2(888, 624), new Vector2(876, 640), 
            new Vector2(853, 663), new Vector2(830, 686), new Vector2(821, 693), new Vector2(811, 700), 
            new Vector2(793, 713), new Vector2(775, 727), new Vector2(757, 738), new Vector2(738, 750),
            new Vector2(718, 760), new Vector2(698, 769), new Vector2(683, 775), new Vector2(669, 782), 
            new Vector2(636, 790), new Vector2(603, 799), new Vector2(579, 802), new Vector2(555, 805), 
            new Vector2(537, 807), new Vector2(518, 808), new Vector2(486, 806), new Vector2(455, 804), 
            new Vector2(442, 799), new Vector2(428, 794), new Vector2(416, 788), new Vector2(403, 782), 
            new Vector2(392, 774), new Vector2(381, 766), new Vector2(372, 757), new Vector2(362, 749), 
            new Vector2(354, 738), new Vector2(347, 728), new Vector2(340, 716), new Vector2(332, 703), 
            new Vector2(326, 688), new Vector2(320, 673), new Vector2(316, 658), new Vector2(312, 643), 
            new Vector2(310, 625), new Vector2(307, 607), new Vector2(307, 592), new Vector2(306, 576), 
            new Vector2(306, 563), new Vector2(306, 551), new Vector2(307, 537), new Vector2(307, 524), 
            new Vector2(310, 509), new Vector2(314, 494), new Vector2(318, 481), new Vector2(323, 468), 
            new Vector2(329, 456), new Vector2(334, 444), new Vector2(340, 433), new Vector2(347, 422), 
            new Vector2(361, 405), new Vector2(375, 389), new Vector2(392, 375), new Vector2(401, 369), 
            new Vector2(409, 364), new Vector2(420, 359), new Vector2(430, 354), new Vector2(440, 351), 
            new Vector2(450, 348), new Vector2(471, 343), new Vector2(493, 339), new Vector2(512, 336), 
            new Vector2(532, 334), new Vector2(534, 334), new Vector2(537, 334), new Vector2(548, 332), 
            new Vector2(559, 331), new Vector2(570, 330), new Vector2(581, 329), new Vector2(587, 329), 
            new Vector2(593, 328), new Vector2(601, 327), new Vector2(609, 326), new Vector2(614, 325), 
            new Vector2(618, 325), new Vector2(621, 325), new Vector2(625, 325), new Vector2(631, 324), 
            new Vector2(637, 323)
        };
    }
}