/*
    Copyright (C) 2015  Yan A. 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Ludus_Latrunculorum
{
    class ImageHandler
    {
        

        private  static  Image  bp       = Properties.Resources.Black_Pawn;
        private  static  Image  bp_high  = Properties.Resources.Black_Pawn_high;
        private  static  Image  bd       = Properties.Resources.Black_Dux;
        private  static  Image  bd_high  = Properties.Resources.Black_Dux_high;
        private  static  Image  wp       = Properties.Resources.White_Pawn;
        private  static  Image  wp_high  = Properties.Resources.White_Pawn_high;
        private  static  Image  wd       = Properties.Resources.White_Dux;
        private  static  Image  wd_high  = Properties.Resources.White_Dux_high;



        // Gets a colour, isDux boolean and isHiglighted boolean, and returns an image accordingly
        public static Image PlaceImage(int colour, bool isDux,bool isHighlighted)
        {
            if(colour == 1) {
                if (isDux) {
                    if (isHighlighted)
                        return wd_high;
                    return wd;
                } else {
                    if (isHighlighted)
                        return wp_high;
                    return wp;
                }
            } else if (colour == -1){
                if (isDux) {
                    if (isHighlighted)
                        return bd_high;
                    return bd;
                } else {
                    if (isHighlighted)
                        return bp_high;
                    return bp;
                }
            }

            return null;
        }
    }
}
