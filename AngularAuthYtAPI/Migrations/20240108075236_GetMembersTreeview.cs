using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularAuthYtAPI.Migrations
{
    public partial class GetMembersTreeview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE   PROCEDURE [dbo].[GetMembersTreeview] 	
                                 @id INT
                                 AS
                                  Begin
                            with tree as
                                 (
                                     select m.Id, u.FirstName+' '+ u.LastName as FullName, m.Name, parentId,1 as tree_order,p.Rate, p.Name as package, path = cast('root' as varchar(100)) 
                                     from   members m
									 join users u on m.UserId = u.Id
									 join plantypes p on m.PlanId = p.Id
                                     where  parentID =@id
                                     union all
                                     select os.Id, u.FirstName+' '+ u.LastName as FullName, os.Name, os.parentId, 1 + tree_order as tree_order, pt.Rate,
                                         pt.Name as package,  path = cast(tree.path + '/' + right(('000000000' + os.Id), 10) as varchar(100))
                                     from   members os
									 join users u on os.UserId = u.Id
									  join plantypes pt on os.PlanId = pt.Id
                                     join   tree 
                                     on     tree.Id = os.parentId
                                 )
                                 select Id,FullName, name as Name, tree_order as TreeOrder,Rate,package as Package, path as Path, t2.cnt as Count, ParentId 
                                 from tree
                                 cross apply (select count(*)-1 cnt from tree t1 where t1.path like tree.path + '%') t2
                                 where id >@id and ParentId >= @id
                                 order by tree_order 
                                  end";


            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop   PROCEDURE [dbo].[GetMembersTreeview] 	
                                 @id INT
                                 AS
                                 Begin
                                 with tree as
                                 (
                                     select Id, Name, parentId,0 as tree_order, path = cast('root' as varchar(100)) 
                                     from   members 
                                     where  parentID is null
                                     union all
                                     select os.Id, os.Name, os.parentId, 1 + tree_order as tree_order,
                                            path = cast(tree.path + '/' + right(('000000000' + os.Id), 10) as varchar(100))
                                     from   members os
                                     join   tree 
                                     on     tree.Id = os.parentId
                                 )
                                 select Id,name as Name, tree_order as TreeOrder, path as Path, t2.cnt as Count, ParentId 
                                 from tree
                                 cross apply (select count(*)-1 cnt from tree t1 where t1.path like tree.path + '%') t2
                                 where id >=@id
                                 order by tree_order 
                                  end";


            migrationBuilder.Sql(procedure);

        }
    }
}
