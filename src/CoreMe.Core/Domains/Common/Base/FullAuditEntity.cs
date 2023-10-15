﻿using FreeSql.DataAnnotations;
using System;

namespace CoreMe.Core.Domains.Common.Base;

#region EntityDto
public interface IEntityDto
{
}

public interface IEntityDto<TKey> : IEntityDto
{
    TKey Id { get; set; }
}

public abstract class EntityDto<TKey> : IEntityDto<TKey>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public TKey Id { get; set; }
}

public abstract class EntityDto : EntityDto<long>
{
}

public class FullEntityDto<TKey> : EntityDto<TKey>, IUpdateAuditEntity, IDeleteAduitEntity, ICreateAduitEntity
{
    /// <summary>
    /// 创建者ID
    /// </summary>
    public long CreateUserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人id
    /// </summary>
    public long? DeleteUserId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 最后修改人Id
    /// </summary>
    public long? UpdateUserId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}

public abstract class FullEntityDto : FullEntityDto<long>
{
}
#endregion EntityDto

#region Entity
public abstract class Entity<TKey> : IEntity<TKey>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Column(IsPrimary = true, IsIdentity = true, Position = 1)]
    public TKey Id { get; set; }
}

[Serializable]
public abstract class Entity : Entity<long>
{
}

public interface IEntity<T>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    T Id { get; set; }
}

public interface IEntity : IEntity<long>
{
}

[Serializable]
public class FullAduitEntity : FullAduitEntity<long>
{
}

public class FullAduitEntity<TKey> : Entity<TKey>, IUpdateAuditEntity, IDeleteAduitEntity, ICreateAduitEntity
{
    /// <summary>
    /// 创建者ID
    /// </summary>

    [Column(Position = -7)]//倒数第七个字段
    public long CreateUserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column(Position = -6)]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [Column(Position = -5)]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人id
    /// </summary>
    [Column(Position = -4)]
    public long? DeleteUserId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [Column(Position = -3)]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 最后修改人Id
    /// </summary>
    [Column(Position = -2)]
    public long? UpdateUserId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Column(Position = -1)]
    public DateTime? UpdateTime { get; set; }
}

public interface ICreateAduitEntity
{
    /// <summary>
    /// 创建者ID
    /// </summary>
    long CreateUserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreateTime { get; set; }
}

public interface IUpdateAuditEntity
{
    /// <summary>
    /// 最后修改人Id
    /// </summary>
    long? UpdateUserId { get; set; }
    /// <summary>
    /// 修改时间
    /// </summary>
    DateTime? UpdateTime { get; set; }
}

public interface IDeleteAduitEntity
{
    /// <summary>
    /// 是否删除
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人id
    /// </summary>
    long? DeleteUserId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeleteTime { get; set; }
}
#endregion Entity
