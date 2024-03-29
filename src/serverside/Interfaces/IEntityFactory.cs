﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Serverside.Interfaces
{
    public interface IEntityFactory<TEntity, TModel>
    {
        TEntity Create(TModel model);
    }
}
